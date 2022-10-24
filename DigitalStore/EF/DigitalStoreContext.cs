using System;
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

        public virtual DbSet<AspUsersCustomer> AspUsersCustomers { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductOrder> ProductOrders { get; set; } = null!;

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

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.CityId, "IX_Customers_CityId");

                entity.Property(e => e.Email).HasColumnName("EMail");

                entity.Property(e => e.FirstName).HasMaxLength(25);

                entity.Property(e => e.LastName).HasMaxLength(25);

                entity.Property(e => e.MidName).HasMaxLength(25);

                entity.Property(e => e.PhoneNumber).HasMaxLength(9);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.CityId, "IX_Orders_CityId");

                entity.HasIndex(e => e.CustomerId, "IX_Orders_CustomerId");

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

                entity.Property(e => e.ProductDescription).HasMaxLength(300);

                entity.Property(e => e.ProductImage).HasDefaultValueSql("(0x)");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.HasIndex(e => e.OrderId, "IX_ProductOrders_OrderId");

                entity.HasIndex(e => e.ProductId, "IX_ProductOrders_ProductId");

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOrders)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            // for Identity
            base.OnModelCreating(modelBuilder);
        }
    }
}
