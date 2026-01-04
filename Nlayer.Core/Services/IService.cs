using System.Linq.Expressions;

namespace Nlayer.Core.Services;

public interface IService<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    
    Task<IEnumerable<T>> GetAllAsync();
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
    
    //Service katmanındaki işlemler genelde veritabanına dokunduğu için void değil Task yapılır.
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
    Task RemoveRangeAsync(IEnumerable<T>  entities);
}