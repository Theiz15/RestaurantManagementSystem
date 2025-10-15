using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Enums;
using RestaurantManagementSystem.Entities;
using Org.BouncyCastle.Asn1.Cms; // Đảm bảo đã include Enums

namespace RestaurantManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets for each entity
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftAssignment> ShiftAssignments { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderTable> OrderTables { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<OrderPromotion> OrderPromotions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }

        public DbSet<InvalidToken> InvalidTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình mối quan hệ và các ràng buộc khác tại đây
            // Ví dụ: Quan hệ nhiều-nhiều cho Order-Table thông qua OrderTable
            modelBuilder.Entity<OrderTable>()
                .HasKey(ot => new { ot.OrderId, ot.TableId }); // Composite Key
            modelBuilder.Entity<OrderTable>()
                .HasOne(ot => ot.Order)
                .WithMany(o => o.OrderTables)
                .HasForeignKey(ot => ot.OrderId);
            modelBuilder.Entity<OrderTable>()
                .HasOne(ot => ot.Table)
                .WithMany(t => t.OrderTables)
                .HasForeignKey(ot => ot.TableId);

            // Quan hệ nhiều-nhiều cho Order-Promotion thông qua OrderPromotion
            modelBuilder.Entity<OrderPromotion>()
                .HasKey(op => new { op.OrderId, op.PromotionId }); // Composite Key
            modelBuilder.Entity<OrderPromotion>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderPromotions)
                .HasForeignKey(op => op.OrderId);
            modelBuilder.Entity<OrderPromotion>()
                .HasOne(op => op.Promotion)
                .WithMany(p => p.OrderPromotions)
                .HasForeignKey(op => op.PromotionId);

            // Quan hệ 1-nhiều giữa User và Payment (Cashier)
            // modelBuilder.Entity<Payment>()
            //     .HasOne(p => p.Cashier)
            //     .WithMany(u => u.Payments)
            //     .HasForeignKey(p => p.CashierId)
            //     .IsRequired(false); // Cashier có thể null nếu thanh toán tự động, v.v.

            // Setting for shift assignment for many-to-many between User and Shift
            modelBuilder.Entity<ShiftAssignment>()
                .HasKey(sa => new { sa.ShiftId, sa.UserId });
            modelBuilder.Entity<ShiftAssignment>()
                .HasOne(sa => sa.Shift)
                .WithMany(s => s.ShiftAssignments)
                .HasForeignKey(sa => sa.ShiftId);
            modelBuilder.Entity<ShiftAssignment>()
                .HasOne(sa => sa.User)
                .WithMany(u => u.ShiftAssignments)
                .HasForeignKey(sa => sa.UserId);


            // Chuyển đổi Enum sang chuỗi khi lưu vào DB (hoặc int tùy chọn)
            modelBuilder.Entity<Shift>()
                .Property(s => s.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Shift>()
                .Property(s => s.StartTime)
                .HasConversion(
                    v => v.ToString(),
                    v => TimeSpan.Parse(v)
                );

            modelBuilder.Entity<Shift>()
                .Property(s => s.EndTime)
                .HasConversion(
                    v => v.ToString(),
                    v => TimeSpan.Parse(v)
                );

            modelBuilder.Entity<Table>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderType)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentMethod)
                .HasConversion<string>(); 

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.OrderId)
                .IsUnique(false);    

            modelBuilder.Entity<InventoryTransaction>()
                .Property(it => it.TransactionType)
                .HasConversion<string>();

            modelBuilder.Entity<FileUpload>()
                .Property(f => f.FileType)
                .HasConversion<string>();

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Manager" },
                new Role { Id = 3, Name = "Staff" },
                new Role { Id = 4, Name = "Customer" }
            );

            modelBuilder.Entity<Shift>().HasData(
                new Shift {Id = 1, Name = "Day Shift", StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0), Status = ShiftStatus.Active},
                new Shift {Id = 2, Name = "Afternoon Shift", StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(17, 0, 0), Status = ShiftStatus.Active},
                new Shift {Id = 3, Name = "Night Shift", StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(22, 0, 0), Status = ShiftStatus.Active} 
            );

        }
    }
}