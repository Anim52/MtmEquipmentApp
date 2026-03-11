using Microsoft.EntityFrameworkCore;
using MtmEquipmentApp.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace MtmEquipmentApp.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Equipment> Equipment => Set<Equipment>();
        public DbSet<Inspection> Inspections => Set<Inspection>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;Database=MTM_EquipmentControlDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(x => x.FullName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.Login)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(x => x.Role)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasIndex(x => x.Login)
                    .IsUnique();
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(x => x.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.InventoryNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.Status)
                    .HasConversion<int>()
                    .IsRequired();

                entity.HasIndex(x => x.InventoryNumber)
                    .IsUnique();
            });

            modelBuilder.Entity<Inspection>(entity =>
            {
                entity.Property(x => x.Remarks)
                    .HasMaxLength(1000);

                entity.Property(x => x.Date)
                    .IsRequired();

                entity.Property(x => x.IsDefective)
                    .IsRequired();

                entity.HasOne(x => x.Equipment)
                    .WithMany(x => x.Inspections)
                    .HasForeignKey(x => x.EquipmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Inspector)
                    .WithMany(x => x.Inspections)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
