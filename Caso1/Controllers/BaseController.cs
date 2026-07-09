using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caso1.Controllers
{
    public class BaseController : Controller
    {
        protected bool ValidateDto<TDto>(TDto dto, AbstractValidator<TDto> validator)
        {
            var context = new ValidationContext<TDto>(dto);
            var result  = validator.Validate(context);

            if (result.IsValid)
            {
                return true;
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return false;
        }
    }
}
