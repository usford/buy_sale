using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace buy_sale.database.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private BuySaleDbContext _db;
        public ProductRepository(BuySaleDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        async public Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.ToArrayAsync();
        }

        async public Task<Product> SingleOrDefaultAsync(int id)
        {
            var product = await _db.Products.SingleOrDefaultAsync(p => p.Id == id);

            return product;
        }

        async public Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        async public Task Add(Product product)
        {
            await _db.Products.AddAsync(product);
        }

        public Task<bool> Update(Product product)
        {
            var result = _db.Products.AsNoTracking().SingleOrDefault(x => x.Id == product.Id);

            if (result is null) return Task.FromResult(false);

            _db.Products.Update(product);

            return Task.FromResult(true);
        }

        public Task<bool> Delete(int id)
        {
            var product = _db.Products.SingleOrDefault(p => p.Id == id);

            if (product is null) return Task.FromResult(false);

            var result = _db.Products.Remove(product);

            return Task.FromResult(true);
        }
    }
}
