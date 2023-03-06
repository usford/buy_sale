using buy_sale.database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection.Emit;

namespace buy_sale.database
{
    public class BuySaleDbContext : DbContext
    {
        private static bool instance = false;
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesPoint> SalesPoints { get; set; }
        public DbSet<ProvidedProducts> ProvidedProducts { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<SalesData> SalesData { get; set; }
        public DbSet<Sale> Sales { get; set; }

        public BuySaleDbContext(DbContextOptions<BuySaleDbContext> options) : base(options)
        {
            if (!instance)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
                instance = true;
            }        
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
                new Product { Id = 1, Name = "Мыло", Price = 0.25m },
                new Product { Id = 2, Name = "Верёвка", Price = 5.25m },
                new Product { Id = 3, Name = "Стул", Price = 10.25m }
            };
            modelBuilder.Entity<Product>().HasData(products);
            
            var salesPoints = new List<SalesPoint>()
            {
                new SalesPoint {Id = 1, Name = "Не кисни - зависни"},
            };
            modelBuilder.Entity<SalesPoint>().HasData(salesPoints);

            var providedProducts = new List<ProvidedProducts>
            {
                new ProvidedProducts { Id = 1, ProductId = 1, ProductQuantity = 6, SalesPointId = 1 },
                new ProvidedProducts { Id = 2, ProductId = 2, ProductQuantity = 3, SalesPointId = 1 },
                new ProvidedProducts { Id = 3, ProductId = 3, ProductQuantity = 12, SalesPointId = 1 }
            };
            modelBuilder.Entity<ProvidedProducts>().HasData(providedProducts);

            var buyers = new List<Buyer>
            {
                new Buyer { Id = 1, Name = "Максим" },
                new Buyer { Id = 2, Name = "Вадим" }
            };
            modelBuilder.Entity<Buyer>().HasData(buyers);

            var sales = new List<Sale>
            {
                new Sale {
                    Id = 1,
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
