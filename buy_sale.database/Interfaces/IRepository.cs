
namespace buy_sale.database.Interfaces
{
    public interface IRepository<T>
    {
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> SingleOrDefaultAsync(int id);
        Task<int> SaveChangesAsync();
    }
}
