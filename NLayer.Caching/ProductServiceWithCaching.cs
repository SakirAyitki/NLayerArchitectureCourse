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

public class ProductServiceWithCaching : IProductService
{
    private const string CacheProductKey = "productsCache";
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;
    private  readonly IMemoryCache _memoryCache;
    private readonly IUnitOfWork _unitOfWork;

    public ProductServiceWithCaching(IMapper mapper, IProductRepository repository, IMemoryCache memoryCache, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _repository = repository;
        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;

        if (!_memoryCache.TryGetValue(CacheProductKey, out _))
        {
            _memoryCache.Set(CacheProductKey, _repository.GetProductsWithCategory().Result);
        }
        
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x=>x.Id==id);
        if (product == null)
        {
            throw new NotFoundException($"Product with id {id} not found");
        }

        return await Task.FromResult(product);
    }
    
    public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
    {
        return await _repository.AnyAsync(expression);
    }

    public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
    {
       return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
    }
    
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await Task.FromResult(_memoryCache.Get<List<Product>>(CacheProductKey));
    }
    
    public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductWithCategory()
    {
        var productsWithCategory = await Task.FromResult(_memoryCache.Get<List<Product>>(CacheProductKey));
        var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(productsWithCategory);
        return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsWithCategoryDto);
    }
    
    public async Task<Product> AddAsync(Product entity)
    {
        await _repository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
        await CacheAllProductsAsync();
        return entity;
    }

    public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
    {
        await _repository.AddRangeAsync(entities);
        await _unitOfWork.CommitAsync();
        await CacheAllProductsAsync();
        return entities;
    }
    
    public async Task UpdateAsync(Product entity)
    {
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
        await CacheAllProductsAsync();
    }

    public async Task RemoveAsync(Product entity)
    {
        _repository.Remove(entity);
        await _unitOfWork.CommitAsync();
        await CacheAllProductsAsync();
    }

    public async Task RemoveRangeAsync(IEnumerable<Product> entities)
    {
        _repository.RemoveRange(entities);
        await _unitOfWork.CommitAsync();
        await CacheAllProductsAsync();
    }
    
    public async Task CacheAllProductsAsync()
    {
        _memoryCache.Set(CacheProductKey,await _repository.GetAll().ToListAsync());
    }
    
}