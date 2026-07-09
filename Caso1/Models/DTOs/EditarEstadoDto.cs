using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caso1.Models.DTOs
{
    public class EditarEstadoDto
    {
        public int EstadoId { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Orden { get; set; }

        public bool Activo { get; set; }
    }
}
