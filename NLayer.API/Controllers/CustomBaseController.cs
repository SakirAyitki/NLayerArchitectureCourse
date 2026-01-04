using Microsoft.AspNetCore.Mvc;
using Nlayer.Core.DTOs;

namespace NLayer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomBaseController : Controller 
{
    [NonAction]
    public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
    {
        if (response.StatusCode == 204)
            return new ObjectResult(null)
            {
                StatusCode = response.StatusCode
            };
        return new ObjectResult(response)
        {
            StatusCode = response.StatusCode
        };
    }
}