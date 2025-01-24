using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System.Data;

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
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<paymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserLikedProduct> UserLikedProducts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>()
                .HasOne(s => s.ServiceType)
                .WithMany(st => st.Services)
                .HasForeignKey(s => s.ServiceTypeId);

            modelBuilder.Entity<ServiceType>()
                .HasOne(st => st.Category)
                .WithMany(c => c.ServiceTypes)
                .HasForeignKey(st => st.CategoryId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)  // Cambié Customer a User
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.User)  // Cambié Customer a User
                .WithMany(u => u.Invoices)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<Invoice>()
            .HasMany(i => i.Orders)
            .WithOne(o => o.Invoice)
            .HasForeignKey(o => o.InvoiceId);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.User)  // Cambié Customer a User
                .WithMany(u => u.ShoppingCarts)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserLikedProduct>()
                .HasKey(ulp => new { ulp.UserId, ulp.ProductId });

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
