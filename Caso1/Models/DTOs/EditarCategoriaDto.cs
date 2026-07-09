using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caso1.Models.DTOs
{
    public class EditarCategoriaDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }
    }
}
