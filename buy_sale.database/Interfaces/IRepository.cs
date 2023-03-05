using buy_sale.database.Models;
using System.Linq.Expressions;

namespace buy_sale.database.Interfaces
{
    public interface IRepository<T>
    {
        //void Add(T entity);
        //Task<T> Update(T entity);
        //Task<bool> Delete(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        //Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
