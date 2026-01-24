using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Nlayer.Core.DTOs;
using Nlayer.Core.Models;
using Nlayer.Core.Repositories;
using Nlayer.Core.Services;
using Nlayer.Core.UnitOfWork;
using NLayer.Service.Exceptions;

namespace NLayer.Caching;

public class CategoryServiceWithCaching : ICategoryService
{
    private const string CategoryCacheKey = "categoryCache";
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;

    public CategoryServiceWithCaching(ICategoryRepository repository, IMapper mapper, IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;

        if (!_memoryCache.TryGetValue(CategoryCacheKey, out _))
        {
            _memoryCache.Set(CategoryCacheKey, repository.GetAll());
        }
        
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        var product = _memoryCache.Get<List<Category>>(CategoryCacheKey).FirstOrDefault(x=>x.Id==id);
        if (product == null)
        {
            throw new NotFoundException($"Product with id {id} not found");
        }

        return await Task.FromResult(product);
    }

    public async Task<Category> AddAsync(Category entity)
    {
        await _repository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
        await CacheAllCategoriesAsync();
        return entity;

    }

    public async Task<bool> AnyAsync(Expression<Func<Category, bool>> expression)
    {
        return await _repository.AnyAsync(expression);
    }

    public async Task<IEnumerable<Category>> AddRangeAsync(IEnumerable<Category> entities)
    {
        await _repository.AddRangeAsync(entities);
        await _unitOfWork.CommitAsync();
        await CacheAllCategoriesAsync();
        return entities;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await Task.FromResult(_memoryCache.Get<IEnumerable<Category>>(CategoryCacheKey));
    }

    public IQueryable<Category> Where(Expression<Func<Category, bool>> expression)
    {
        return _memoryCache.Get<List<Category>>(CategoryCacheKey).Where(expression.Compile()).AsQueryable();
    }

    public async Task UpdateAsync(Category entity)
    {
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
        await CacheAllCategoriesAsync();
    }

    public async Task RemoveAsync(Category entity)
    {
        _repository.Remove(entity);
        await _unitOfWork.CommitAsync();
        await CacheAllCategoriesAsync();
    }

    public async Task RemoveRangeAsync(IEnumerable<Category> entities)
    {
        _repository.RemoveRange(entities);
        await _unitOfWork.CommitAsync();
        await CacheAllCategoriesAsync();
    }

    public async Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProductAsync(int categoryId)
    {
        var categories = _memoryCache.Get<List<Category>>(CategoryCacheKey);

        if (categories == null)
        {
            categories = await _repository
                .GetAll()
                .ToListAsync();

            _memoryCache.Set(CategoryCacheKey, categories);
        }

        var category = categories.FirstOrDefault(x => x.Id == categoryId);

        if (category == null)
        {
            throw new NotFoundException($"Category with id {categoryId} not found");
        }

        var categoryDto = _mapper.Map<CategoryWithProductsDto>(category);

        return CustomResponseDto<CategoryWithProductsDto>.Success(200, categoryDto);
    }

    public async Task CacheAllCategoriesAsync()
    {
        _memoryCache.Set(CategoryCacheKey, await _repository.GetAll().ToListAsync());
    }
}