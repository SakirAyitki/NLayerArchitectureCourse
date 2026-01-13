using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nlayer.Core.DTOs;

namespace NLayer.API.Filters;

    public class ValidateFilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //contex uzerinden validation (hata) var mi yok mu check et
            //su an kosul ==> bir hata varsa
            if(!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x=>x.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(400, errors));
            }
        }
    }