using System;
using System.ComponentModel.DataAnnotations;
using Caso1.Common;
using Caso1.Models.Identity;

namespace Caso1.Models.Entities
{
    public class Tarea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El titulo es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El titulo debe tener entre 3 y 100 caracteres")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El detalle es obligatorio")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "El detalle debe tener entre 5 y 500 caracteres")]
        [Display(Name = "Detalle")]
        public string Detalle { get; set; }

        [Required(ErrorMessage = "La fecha y hora es obligatoria")]
        [Display(Name = "Fecha y Hora")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public EstadoTarea Estado { get; set; }

        public bool Borrado { get; set; } = false;

        // Dueño de la tarea. Sin [Required] y sin StringLength con
        // MinimumLength para que sea opcional (nullable) y no truene
        // con las tareas que ya existen en la base de datos.
        // La relación FK con ApplicationUser se configura por Fluent API
        // en Caso1DbContext.OnModelCreating (no se usa [ForeignKey] aquí
        // porque choca con un tipo duplicado dentro de EntityFramework.dll).
        [StringLength(128)]
        public string UsuarioId { get; set; }

        public virtual ApplicationUser Usuario { get; set; }

        // Rol con el que el dueño decide compartir la tarea (opcional).
        // Si es null la tarea es privada: solo la ve el dueño y el
        // Administrador. Si tiene un valor (ej. "Soporte"), también la
        // pueden ver los usuarios que pertenezcan a ese rol.
        [StringLength(256)]
        [Display(Name = "Compartir con rol")]
        public string RolCompartido { get; set; }

        public Tarea()
        {
            FechaHora = DateTime.Now;
            Estado = EstadoTarea.Pendiente;
        }
    }
}
