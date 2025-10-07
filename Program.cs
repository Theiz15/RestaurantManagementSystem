using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Exception;
using RestaurantManagementSystem.Mappings;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using RestaurantManagementSystem.Services;
using RestaurantManagementSystem.Services.Impl;
using RestaurantManagementSystem.Services.Storage;
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
//builder.Services.AddSwaggerGen();

// 2. Register Repositories and Services for Dependency Injection
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryServiceImpl>();
// ... Register other repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();
builder.Services.AddScoped<IFileUploadService, FileUploadServiceImpl>();
builder.Services.AddSingleton<IFileStorageService, LocalStorageService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();

// Đăng ký Service
builder.Services.AddScoped<IMenuItemService, MenuItemServiceImpl>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMenuItemService, MenuItemServiceImpl>();
// ... Register other services
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Lấy connection string từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Thêm DbContext và cấu hình MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); builder.Services.AddSwaggerGen();


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

// Register MailService
builder.Services.AddScoped<MailService, MailServiceImpl>();


var app = builder.Build();

//Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//`app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
