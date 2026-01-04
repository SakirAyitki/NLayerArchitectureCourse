using Nlayer.Core.Models;

namespace Nlayer.Core.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<List<Product>> GetProductsWithCategory();
}