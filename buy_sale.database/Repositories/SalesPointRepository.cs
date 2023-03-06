using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using Microsoft.EntityFrameworkCore;

namespace buy_sale.database.Repositories
{
    public class SalesPointRepository : IRepository<SalesPoint>
    {
        private BuySaleDbContext _db;
        public SalesPointRepository(BuySaleDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        async public Task<IEnumerable<SalesPoint>> GetAllAsync()
        {
            return await _db.SalesPoints
                .AsNoTracking()
                .Include(sp => sp.ProvidedProducts)
                .ThenInclude(pp => pp.Product)
                .ToArrayAsync();
        }

        async public Task<SalesPoint> SingleOrDefaultAsync(int id)
        {
            var salesPoint = await _db.SalesPoints
                .AsNoTracking()
                .Include(sp => sp.ProvidedProducts)
                .ThenInclude(pp => pp.Product)
                .SingleOrDefaultAsync(sp => sp.Id == id);

            return salesPoint;
        }

        async public Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        async public Task<bool> Add(SalesPoint salesPoint)
        {
            var addedSalesPoint = await _db.SalesPoints.AddAsync(salesPoint);
            var products = _db.Products.ToList();

            foreach (var pp in salesPoint.ProvidedProducts)
            {
                var findProduct = products.Find(p => p.Id == pp.ProductId);
                if (findProduct is null) return false;

                pp.SalesPointId = addedSalesPoint.Entity.Id;
            }
            
            await _db.ProvidedProducts.AddRangeAsync(salesPoint.ProvidedProducts);

            return true;
        }

        public Task<bool> Update(SalesPoint salesPoint)
        {
            var result = _db.SalesPoints.AsNoTracking().SingleOrDefault(sp => sp.Id == salesPoint.Id);

            if (result is null) return Task.FromResult(false);

            _db.SalesPoints.Update(salesPoint);

            return Task.FromResult(true);
        }

        public Task<bool> Delete(int id)
        {
            var salesPoint = _db.SalesPoints.SingleOrDefault(sp => sp.Id == id);

            if (salesPoint is null) return Task.FromResult(false);

            var result = _db.SalesPoints.Remove(salesPoint);

            return Task.FromResult(true);
        }
    }
}
