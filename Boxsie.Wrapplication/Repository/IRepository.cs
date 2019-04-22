using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boxsie.Wrapplication.Repository
{
    public interface IRepository<T>
    {
        Task CreateTable();
        Task<int> CreateAsync(T entity);
        Task<IEnumerable<int>> CreateAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetLastEntriesAsync(int limit = 0);
        Task<IEnumerable<T>> GetWhereAsync(List<WhereClause> where);
    }
}