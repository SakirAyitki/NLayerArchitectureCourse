using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nlayer.Core.Services;

namespace NLayer.API.Controllers;

public class CategoryController:CustomBaseController
{
    private readonly ICategoryService _service;
    private readonly IMapper _mapper;
    
    public CategoryController(ICategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("[action]/{categoryId}")]
    public async Task<IActionResult> GetCategoriesWithProducts(int categoryId)
    {
        return CreateActionResult(await _service.GetSingleCategoryByIdWithProductAsync(categoryId));
    }
}