﻿using System;
using DigitalStore.Identity;
using DigitalStore.Models;
using DigitalStore.Models.NotForDB;
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

        public virtual DbSet<AspUsersCustomer> AspUsersCustomers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOrder> ProductOrders { get; set; }

        public string GetTableName(Type type)
        {
            return Model.FindEntityType(type).GetTableName();// GetAnnotation("Relational:TableName").Value.ToString();  // .SqlServer().TableName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"server=LAPTOP-GQRGKLN2;database=DigitalStore;integrated security=True;
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

            modelBuilder.Entity<AspUsersCustomer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.AspUsersCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspUsersCustomers_Customers");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspUsersCustomers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspUsersCustomers_AspNetUsers");
            });

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
