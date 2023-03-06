
using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using Microsoft.EntityFrameworkCore;

namespace buy_sale.database.Repositories
{
    public class BuyersRepository : IRepository<Buyer>
    {
        private BuySaleDbContext _db;
        public BuyersRepository(BuySaleDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        async public Task<IEnumerable<Buyer>> GetAllAsync()
        {
            return await _db.Buyers
                .AsNoTracking()
                .Include(b => b.Sales)
                    .ThenInclude(s => s.SalesData)
                        .ThenInclude(sd => sd.Product)
                .Include(b => b.Sales)
                    .ThenInclude(s => s.SalesPoint)
                        .ThenInclude(sp => sp.ProvidedProducts)
                            .ThenInclude(pp => pp.Product)
                .ToArrayAsync();
        }

        async public Task<Buyer> SingleOrDefaultAsync(int id)
        {
            return await _db.Buyers
                .AsNoTracking()
                .Include(b => b.Sales)
                    .ThenInclude(s => s.SalesData)
                        .ThenInclude(sd => sd.Product)
                .Include(b => b.Sales)
                    .ThenInclude(s => s.SalesPoint)
                        .ThenInclude(sp => sp.ProvidedProducts)
                            .ThenInclude(pp => pp.Product)
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        async public Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        async public Task<bool> Add(Buyer buyer)
        {
            await _db.Buyers.AddAsync(buyer);

            return true;
        }

        public Task<bool> Update(Buyer buyer)
        {
            var result = _db.Buyers.AsNoTracking().SingleOrDefault(b => b.Id == buyer.Id);

            if (result is null) return Task.FromResult(false);

            _db.Buyers.Update(buyer);

            return Task.FromResult(true);
        }

        public Task<bool> Delete(int id)
        {
            var buyer = _db.Buyers.SingleOrDefault(b => b.Id == id);

            if (buyer is null) return Task.FromResult(false);

            var result = _db.Buyers.Remove(buyer);

            return Task.FromResult(true);
        }
    }
}
