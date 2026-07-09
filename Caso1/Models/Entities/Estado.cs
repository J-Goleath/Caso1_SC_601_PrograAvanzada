using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Caso1.Models.Entities
{
    public class Estado
    {
        [Key]
        public int EstadoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(200, MinimumLength = 5,
            ErrorMessage = "La descripcion debe tener entre 5 y 200 caracteres.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El orden es obligatorio.")]
        [Range(1, 999, ErrorMessage = "El orden debe estar entre 1 y 999.")]
        [Display(Name = "Orden")]
        public int Orden { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }
}