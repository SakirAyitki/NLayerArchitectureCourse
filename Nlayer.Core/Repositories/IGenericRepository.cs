using System.Linq.Expressions;

namespace Nlayer.Core.Repositories;

public interface IGenericRepository <T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    
    //Expressin keywordu SQL Sorgusunun nasıl çalışacağını belirtir (örneğin: x => x.Price > 100)
    //IQueryable'da DB'de çalışacağını belirtmek için. Yani nerede çalışacağı IQueryable nasıl çalışacağını Expression söylüyor.
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task AddRangeAsync(IEnumerable<T> entities);
    
    // IQueryable yapmamızın nedeni SQL sorgusunu DB'de çalıştırıp sorgulanan öğeleri getirmek.
    // IQueryable yapmasaydık önce tüm tabloyu çekip sonra sorgu yapacaktı od ahem yavaş hem sistemi yoruyor.
    IQueryable<T> GetAll();
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
    
    //Update ve Remove için Entity Framework'te async fonksiyonları yok onun için async yapmadık.
    //Uzun süren bir işlem olmadığından async methodu oluşturulmamış, gereksiz yani. 
    //Memory de state değişikliği yapar DB'ye yazma olmaz
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T>  entities);
}