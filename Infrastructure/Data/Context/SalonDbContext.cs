﻿using Microsoft.EntityFrameworkCore;
using Domain.Entities;


namespace Infrastructure.Data.Context
{
    public class SalonDbContext : DbContext
    {
        public SalonDbContext(DbContextOptions<SalonDbContext> options) : base(options) { }
        //public SalonDbContext() : base(new DbContextOptionsBuilder<SalonDbContext>().UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\uriel\\Downloads\\SalonPro\\Infrastructure\\SalonProDB.mdf;Integrated Security=True").Options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string con = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
            optionsBuilder.UseSqlServer(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\uriel\\Downloads\\SalonPro\\Infrastructure\\SalonProDB.mdf;Integrated Security=True",
                b => b.MigrationsAssembly("Infrastructure")); // Asegúrate de usar el nombre del ensamblado donde quieres las migraciones
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffService> StaffServices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffService>()
                .HasKey(ss => new { ss.StaffId, ss.ServiceId });

            modelBuilder.Entity<Service>()
                .HasOne(s => s.ServiceType)
                .WithMany(st => st.Services)
                .HasForeignKey(s => s.ServiceTypeId);

            modelBuilder.Entity<ServiceType>()
                .HasOne(st => st.Category)
                .WithMany(c => c.ServiceTypes)
                .HasForeignKey(st => st.CategoryId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Staff)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.StaffId);

            base.OnModelCreating(modelBuilder);
        }
    }
}