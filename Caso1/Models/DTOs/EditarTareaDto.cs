using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caso1.Models.DTOs
{
    public class EditarTareaDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Detalle { get; set; }

        public string Estado { get; set; }
    }
}
