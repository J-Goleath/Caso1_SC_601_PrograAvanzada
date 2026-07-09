using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caso1.Models.DTOs;

namespace Caso1.Validators
{
    public class CrearPrioridadValidator : AbstractValidator<CrearPrioridadDto>
    {
        public CrearPrioridadValidator()
        {
            RuleFor(r => r.Nombre)
                .NotEmpty().WithMessage("El nombre de la prioridad es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(r => r.Descripcion)
                .NotEmpty().WithMessage("La descripcion es obligatoria.")
                .MinimumLength(5).WithMessage("La descripcion debe tener al menos 5 caracteres.")
                .MaximumLength(500).WithMessage("La descripcion no puede exceder los 500 caracteres.");
        }
    }
}
