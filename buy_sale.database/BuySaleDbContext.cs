﻿using buy_sale.database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection.Emit;

namespace buy_sale.database
{
    public class BuySaleDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesPoint> SalesPoints { get; set; }
        public DbSet<ProvidedProducts> ProvidedProducts { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<SalesData> SalesData { get; set; }
        public DbSet<Sale> Sales { get; set; }

        public BuySaleDbContext(DbContextOptions<BuySaleDbContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            FillingDatabase(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public void FillingDatabase(ModelBuilder modelBuilder)
        {
            var products = new List<Product>()
            {
                new Product { Name = "Мыло", Price = 0.25m },
                new Product { Name = "Верёвка", Price = 5.25m },
                new Product { Name = "Стул", Price = 10.25m }
            };
            modelBuilder.Entity<Product>().HasData(products);

            var salesPoints = new List<SalesPoint>()
            {
                new SalesPoint {Name = "Не кисни - зависни"},
            };
            modelBuilder.Entity<SalesPoint>().HasData(salesPoints);

            var providedProducts = new List<ProvidedProducts>
            {
                new ProvidedProducts { Id = 1, ProductId = products[0].Id, ProductQuantity = 6, SalesPointId = salesPoints[0].Id },
                new ProvidedProducts { Id = 2, ProductId = products[1].Id, ProductQuantity = 3, SalesPointId = salesPoints[0].Id },
                new ProvidedProducts { Id = 3, ProductId = products[2].Id, ProductQuantity = 12, SalesPointId = salesPoints[0].Id }
            };
            modelBuilder.Entity<ProvidedProducts>().HasData(providedProducts);

            var buyers = new List<Buyer>
            {
                new Buyer { Name = "Максим" },
                new Buyer { Name = "Вадим" }
            };
            modelBuilder.Entity<Buyer>().HasData(buyers);

            var sales = new List<Sale>
            {
                new Sale {
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Time = TimeOnly.FromDateTime(DateTime.Now),
                    SalesPointId = salesPoints[0].Id,
                    BuyerId = buyers[0].Id,
                    TotalAmount = 25.75m
                }
            };

            modelBuilder.Entity<Sale>(s =>
            {
                s.Property(s => s.Date).HasConversion<DateOnlyConverter>();
                s.Property(s => s.Time).HasConversion<TimeOnlyConverter>();
                s.HasData(sales);
            });

            var saledData = new List<SalesData>
            {
                new SalesData {
                    Id = 1,
                    ProductId = products[1].Id,
                    ProductQuantity = 1,
                    ProductAmount = 5.25m,
                    SaleId = sales[0].Id },

                new SalesData {
                    Id = 2,
                    ProductId = products[2].Id,
                    ProductQuantity = 2,
                    ProductAmount = 20.5m,
                    SaleId = sales[0].Id }
            };

            modelBuilder.Entity<SalesData>().HasData(saledData);
        }

        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() : base(
                    dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
                    dateTime => DateOnly.FromDateTime(dateTime))
            {
            }
        }

        public class DateOnlyComparer : ValueComparer<DateOnly>
        {
            public DateOnlyComparer() : base(
                (d1, d2) => d1.DayNumber == d2.DayNumber,
                d => d.GetHashCode())
            {
            }
        }

        public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
        {
            public TimeOnlyConverter() : base(
                    timeOnly => timeOnly.ToTimeSpan(),
                    timeSpan => TimeOnly.FromTimeSpan(timeSpan))
            {
            }
        }

        public class TimeOnlyComparer : ValueComparer<TimeOnly>
        {
            public TimeOnlyComparer() : base(
                (t1, t2) => t1.Ticks == t2.Ticks,
                t => t.GetHashCode())
            {
            }
        }
    }
}