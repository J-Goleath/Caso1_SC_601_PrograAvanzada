using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Caso1.Models.Roles
{
    // ViewModel que usa RoleController para mostrar y administrar
    // los roles de un usuario específico (asignar / remover).
    public class AdministrarRolesViewModel
    {
        [Required]
        public string UsuarioId { get; set; }

        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; }

        public IList<string> RolesActuales { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        [Display(Name = "Rol")]
        public string RolSeleccionado { get; set; }

        public IEnumerable<SelectListItem> RolesDisponibles { get; set; }
    }
}
