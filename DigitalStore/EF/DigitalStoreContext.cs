﻿using System;
using DigitalStore.Identity;
using DigitalStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DigitalStore.EF
{
    public class DigitalStoreContext : IdentityDbContext<ApplicationUser>
    {
        public DigitalStoreContext()
        {

        }

        public DigitalStoreContext(DbContextOptions<DigitalStoreContext> options) : base(options)
        {
        }

        public DbSet<DigitalStore.Models.Customer> Customers { get; set; }
        public DbSet<DigitalStore.Models.Category> Categories { get; set; }
        public DbSet<DigitalStore.Models.City> Cities { get; set; }
        public DbSet<DigitalStore.Models.Order> Orders { get; set; }
        public DbSet<DigitalStore.Models.Product> Products { get; set; }
        public DbSet<DigitalStore.Models.ProductOrder> ProductOrders { get; set; }

        public string GetTableName(Type type)
        {
            return Model.FindEntityType(type).GetTableName();// GetAnnotation("Relational:TableName").Value.ToString();  // .SqlServer().TableName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"server=DESKTOP-QT7HQTJ;database=DigitalStore;integrated security=True;
                    MultipleActiveResultSets=True;App=EntityFramework;";
                optionsBuilder.UseSqlServer(
                    connectionString,
                    options => options.EnableRetryOnFailure())
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryPossibleExceptionWithAggregateOperatorWarning));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //create the multi column index
            //modelBuilder.Entity<Customer>(entity =>
            //{
            //    entity.HasIndex(e => new { e.FirstName, e.MidName, e.LastName }).IsUnique();

            //});
            //set the cascade options on the relationship
            modelBuilder.Entity<Product>()
                .HasOne(e => e.Category)
                .WithMany(e => e.Products)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ProductOrder>()
                .HasOne(e => e.Product)
                .WithMany(e => e.ProductOrders)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ProductOrder>()
                .HasOne(e => e.Order)
                .WithMany(e => e.ProductOrders)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Order>()
                .HasOne(e => e.Customer)
                .WithMany(e => e.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Order>()
                .HasOne(e => e.City)
                .WithMany(e => e.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Customer>()
                .HasOne(e => e.City)
                .WithMany(e => e.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // for Identity
            base.OnModelCreating(modelBuilder);
        }
    }
}
