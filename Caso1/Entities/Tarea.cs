using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caso1.Entities
{
    public enum EstadoTarea
    {
        Pendiente = 0,
        EnProceso = 1,
        Completada = 2,
        Cancelada = 3
    }

  
    public class Tarea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 100 caracteres")]
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

        public Tarea()
        {
            FechaHora = DateTime.Now;
            Estado = EstadoTarea.Pendiente;
        }
    }
}
