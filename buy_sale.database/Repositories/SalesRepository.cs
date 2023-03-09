using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace buy_sale.database.Repositories
{
    public class SalesRepository : IRepository<Sale>
    {
        private BuySaleDbContext _db;
        public SalesRepository(BuySaleDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<bool> Add(Sale sale)
        {
            await _db.Sales.AddAsync(sale);

            return true;
        }

        public Task<bool> Delete(int id)
        {
            var sale = _db.Sales.SingleOrDefault(s => s.Id == id);

            if (sale is null) return Task.FromResult(false);

            var result = _db.Sales.Remove(sale);

            return Task.FromResult(true);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _db.Sales
                .Include(s => s.SalesPoint)
                .Include(s => s.Buyer)
                .Include(s => s.SalesData)
                    .ThenInclude(sd => sd.Product)
                .ToArrayAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public async Task<Sale> SingleOrDefaultAsync(int id)
        {
            return await _db.Sales
                .Include(s => s.SalesPoint)
                .Include(s => s.Buyer)
                .Include(s => s.SalesData)
                    .ThenInclude(sd => sd.Product)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public Task<bool> Update(Sale sale)
        {
            var result = _db.Sales.AsNoTracking().SingleOrDefault(s => s.Id == sale.Id);

            if (result is null) return Task.FromResult(false);

            _db.Sales.Update(sale);

            return Task.FromResult(true);
        }
    }
}
