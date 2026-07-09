using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caso1.Models.DTOs;

namespace Caso1.Validators
{
    public class CrearTareaValidator : AbstractValidator<CrearTareaDto>
    {
        public CrearTareaValidator()
        {
            RuleFor(r => r.Titulo)
                .NotEmpty().WithMessage("El titulo es obligatorio.")
                .MinimumLength(3).WithMessage("El titulo debe tener al menos 3 caracteres.")
                .MaximumLength(100).WithMessage("El titulo no puede exceder los 100 caracteres.");

            RuleFor(r => r.Detalle)
                .NotEmpty().WithMessage("El detalle es obligatorio.")
                .MinimumLength(5).WithMessage("El detalle debe tener al menos 5 caracteres.")
                .MaximumLength(500).WithMessage("El detalle no puede exceder los 500 caracteres.");
        }
    }
}
