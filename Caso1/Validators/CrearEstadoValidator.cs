using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caso1.Models.DTOs;

namespace Caso1.Validators
{
    public class CrearEstadoValidator : AbstractValidator<CrearEstadoDto>
    {
        public CrearEstadoValidator()
        {
            RuleFor(r => r.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
                .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres.");

            RuleFor(r => r.Descripcion)
                .MaximumLength(200).WithMessage("La descripcion no puede exceder los 200 caracteres.")
                .MinimumLength(5).When(r => !string.IsNullOrEmpty(r.Descripcion))
                    .WithMessage("La descripcion debe tener al menos 5 caracteres.");

            RuleFor(r => r.Orden)
                .InclusiveBetween(1, 999).WithMessage("El orden debe estar entre 1 y 999.");
        }
    }
}
