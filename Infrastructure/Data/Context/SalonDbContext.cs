using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data.Context
{
    public class SalonDbContext : DbContext
    {
        public SalonDbContext(DbContextOptions<SalonDbContext> options) : base(options) { }

        public SalonDbContext()
            : base(new DbContextOptionsBuilder<SalonDbContext>()
                   .UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\uriel\\Downloads\\SalonPro\\Infrastructure\\SalonProDB.mdf;Integrated Security=True")
                   .Options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\uriel\\Downloads\\SalonPro\\Infrastructure\\SalonProDB.mdf;Integrated Security=True",
                b => b.MigrationsAssembly("Infrastructure"));
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<CartStatus> CartStatuses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserLikedProduct> UserLikedProducts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de Service
            modelBuilder.Entity<Service>()
                .HasOne(s => s.ServiceType)
                .WithMany(st => st.Services)
                .HasForeignKey(s => s.ServiceTypeId);

            // Configuración de ServiceType
            modelBuilder.Entity<ServiceType>()
                .HasOne(st => st.Category)
                .WithMany(c => c.ServiceTypes)
                .HasForeignKey(st => st.CategoryId);

            // Configuración de Appointment
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId);

            // Configuración de ShoppingCart
            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.ToTable("ShoppingCart");

                entity.HasKey(sc => sc.Id);

                entity.HasOne(sc => sc.User)
                    .WithMany(u => u.ShoppingCarts)
                    .HasForeignKey(sc => sc.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sc => sc.CartStatus)
                    .WithMany()
                    .HasForeignKey(sc => sc.CartStatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de CartDetails
            modelBuilder.Entity<CartDetail>(entity =>
            {
                entity.ToTable("CartDetails");

                entity.HasKey(cd => cd.Id);

                entity.HasOne(cd => cd.ShoppingCart)
                    .WithMany(sc => sc.CartDetails)
                    .HasForeignKey(cd => cd.CartId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cd => cd.Product)
                    .WithMany(p => p.CartDetails)
                    .HasForeignKey(cd => cd.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de UserLikedProduct
            modelBuilder.Entity<UserLikedProduct>()
                .HasKey(ulp => new { ulp.UserId, ulp.ProductId });

            // Configuración de UserRole
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);
            });

            // Configuración de Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.HasKey(o => o.Id);

                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.State)
                    .WithMany()
                    .HasForeignKey(o => o.StateId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.PaymentMethod)
                    .WithMany()
                    .HasForeignKey(o => o.PaymentMethodId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Cart)
                    .WithMany()
                    .HasForeignKey(o => o.CartId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // Configuración de OrderDetails
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails");

                entity.HasKey(od => od.Id);

                entity.HasOne(od => od.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(od => od.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(od => od.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(od => od.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de OrderStatus
            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");

                entity.HasKey(os => os.Id);

                entity.Property(os => os.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(os => os.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configuración de CartStatus
            modelBuilder.Entity<CartStatus>(entity =>
            {
                entity.ToTable("CartStatus");

                entity.HasKey(cs => cs.Id);

                entity.Property(cs => cs.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(cs => cs.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
