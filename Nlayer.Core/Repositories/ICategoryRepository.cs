using Nlayer.Core.DTOs;
using Nlayer.Core.Models;

namespace Nlayer.Core.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category> GetSingleCategoryByIdWithProductAsync(int categoryId);
}