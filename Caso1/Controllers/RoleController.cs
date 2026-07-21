using Caso1.Common;
using Caso1.Infrastructure.DbContexts;
using Caso1.Logging;
using Caso1.Models.Identity;
using Caso1.Models.Roles;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

namespace Caso1.Controllers
{

    [Authorize(Roles = Roles.Administrador)]
    public class RoleController : Controller
    {
        private readonly Caso1DbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController()
        {
            _context = new Caso1DbContext();
            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(_context));
            _roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(_context));
        }

        public ActionResult Index()
        {
            AppLogger.Info("Acceso al listado de usuarios y roles");

            var usuarios = _context.Users
                .OrderBy(u => u.Email)
                .ToList();

            return View(usuarios);
        }

        public ActionResult Administrar(string id)
        {
            var usuario = _userManager.FindById(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            AppLogger.Info(
                "Acceso a administración de roles del usuario {Email}",
                usuario.Email);

            return View(ConstruirViewModel(usuario));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarRol(AdministrarRolesViewModel model)
        {
            var usuario = _userManager.FindById(model.UsuarioId);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            if (!_roleManager.RoleExists(model.RolSeleccionado))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "El rol seleccionado no existe.");

                return View("Administrar", ConstruirViewModel(usuario));
            }

            if (!_userManager.IsInRole(usuario.Id, model.RolSeleccionado))
            {
                var resultado = _userManager.AddToRole(
                    usuario.Id,
                    model.RolSeleccionado);

                if (!resultado.Succeeded)
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }

                    return View("Administrar", ConstruirViewModel(usuario));
                }

                AppLogger.Info(
                    "Rol {Rol} asignado al usuario {Email}",
                    model.RolSeleccionado,
                    usuario.Email);
            }

            TempData["MensajeExito"] = "El rol fue asignado correctamente.";

            return RedirectToAction(
                "Administrar",
                new { id = usuario.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverRol(string usuarioId, string rolNombre)
        {
            var usuario = _userManager.FindById(usuarioId);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            if (_userManager.IsInRole(usuario.Id, rolNombre))
            {
                var resultado = _userManager.RemoveFromRole(
                    usuario.Id,
                    rolNombre);

                if (resultado.Succeeded)
                {
                    AppLogger.Info(
                        "Rol {Rol} removido del usuario {Email}",
                        rolNombre,
                        usuario.Email);

                    TempData["MensajeExito"] =
                        "El rol fue removido correctamente.";
                }
                else
                {
                    TempData["MensajeError"] =
                        string.Join(" ", resultado.Errors);
                }
            }

            return RedirectToAction(
                "Administrar",
                new { id = usuario.Id });
        }

        [NonAction]
        private AdministrarRolesViewModel ConstruirViewModel(
            ApplicationUser usuario)
        {
            var rolesActuales = _userManager.GetRoles(usuario.Id);

            return new AdministrarRolesViewModel
            {
                UsuarioId = usuario.Id,
                Email = usuario.Email,
                NombreCompleto = $"{usuario.Nombre} {usuario.Apellidos}",
                RolesActuales = rolesActuales,
                RolesDisponibles = _roleManager.Roles
                    .OrderBy(r => r.Name)
                    .Select(r => new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Name
                    })
                    .ToList()
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager?.Dispose();
                _roleManager?.Dispose();
                _context?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
