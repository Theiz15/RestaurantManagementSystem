using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Exception;
using RestaurantManagementSystem.Mappings;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using RestaurantManagementSystem.Services;
using RestaurantManagementSystem.Services.Impl;
using RestaurantManagementSystem.Services.Storage;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Controllers and configure JSON options to handle enums as strings and avoid cycles
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// 2. Register Repositories and Services for Dependency Injection
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();

builder.Services.AddScoped<ICategoryService, CategoryServiceImpl>();

builder.Services.AddScoped<RestaurantManagementSystem.Services.MailService, MailServiceImpl>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<DatabaseInitializer>();
builder.Services.AddScoped<InvalidTokenRepository>();

// ... Register other repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();
builder.Services.AddScoped<IFileUploadService, FileUploadServiceImpl>();
builder.Services.AddSingleton<IFileStorageService, LocalStorageService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<ShiftRepository>();
builder.Services.AddScoped<ShiftAssignmentRepository>();

// Đăng ký Service
builder.Services.AddScoped<IMenuItemService, MenuItemServiceImpl>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMenuItemService, MenuItemServiceImpl>();
builder.Services.AddScoped<ShiftAssignmentService, ShiftAssignmentServiceImpl>();

builder.Services.AddScoped<AuthenticationService>();

// ... Register other services
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Lấy connection string từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Thêm DbContext và cấu hình MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); 



// 3. Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// 4. Register Global Exception Filter
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

// 5. Register Mail Service and SmtpSettings
// Bind config
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));


// 6. Load appsettings.Secret.json
builder.Configuration.AddJsonFile("appsettings.Secret.json", optional: true, reloadOnChange: true);

//Add middleware for authentication and authorization

// 7. Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            RoleClaimType = "Role",
            NameClaimType = "Username"
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                try
                {
                    var invalidTokenRepo = context.HttpContext.RequestServices
                    .GetRequiredService<InvalidTokenRepository>();

                    var jwtToken = context.SecurityToken as JwtSecurityToken;
                    var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value
                        ?? context.Principal?.FindFirst("jti")?.Value
                        ?? jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value
                        ?? jwtToken?.Id;
                    Console.WriteLine("JTI: " + jti);

                    if (!string.IsNullOrEmpty(jti))
                    {
                        bool isInvalidated = await invalidTokenRepo.ExistsAsync(jti);
                        if (isInvalidated)
                        {
                            context.Fail("This token has been invalidated (user logged out).");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception if needed
                    context.Fail("An error occurred while validating the token.");
                }
            },
            OnAuthenticationFailed = context =>
            {
                return Task.CompletedTask;
            }
        };
    });

// 8. Add Authorization
builder.Services.AddAuthorization(options =>
{
    // Có thể thêm policy riêng nếu cần
    options.AddPolicy("RequireAdminRole",
        policy => policy.RequireRole("Admin"));
});

var app = builder.Build();


// Call the database initializer to seed the admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var initializer = services.GetRequiredService<DatabaseInitializer>();
        var context = services.GetRequiredService<AppDbContext>();
        initializer.SeedAdmin(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}

  


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
