using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using Microsoft.EntityFrameworkCore;


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
    }
}
