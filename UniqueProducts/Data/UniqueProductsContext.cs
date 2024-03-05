using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using UniqueProducts.Models;
using Microsoft.AspNetCore.Identity;

namespace UniqueProducts.Data
{
    public class UniqueProductsContext : IdentityDbContext<IdentityUser>
    {
        public UniqueProductsContext(DbContextOptions<UniqueProductsContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Material> Materials { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");

                entity.Property(e => e.Company).HasColumnName("Company").HasMaxLength(30);
                entity.Property(e => e.Representative).HasColumnName("Representative").HasMaxLength(30);
                entity.Property(e => e.Phone).HasColumnName("Phone").HasMaxLength(30);
                entity.Property(e => e.CompanyAddress).HasColumnName("CompanyAddress").HasMaxLength(60);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.HasOne(d => d.Client)
                    .WithMany()
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Employee)
                    .WithMany()
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.Property(e => e.ProductName).HasColumnName("ProductName").HasMaxLength(30);
                entity.Property(e => e.ProductDescript).HasColumnName("ProductDescript").HasMaxLength(60);
                entity.Property(e => e.ProductColor).HasColumnName("ProductColor").HasMaxLength(30);

                entity.HasOne(d => d.Material)
                    .WithMany()
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable("Materials");

                entity.Property(e => e.MaterialName).HasColumnName("MaterialName").HasMaxLength(30);
                entity.Property(e => e.MaterialDescript).HasColumnName("MaterialDescript").HasMaxLength(60);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");

                entity.Property(e => e.EmployeeName).HasColumnName("EmployeeName").HasMaxLength(30);
                entity.Property(e => e.EmployeeSurname).HasColumnName("EmployeeSurname").HasMaxLength(30);
                entity.Property(e => e.EmployeeMidname).HasColumnName("EmployeeMidname").HasMaxLength(30);
                entity.Property(e => e.EmployeePosition).HasColumnName("EmployeePosition").HasMaxLength(30);
            });
        }
    }
}
