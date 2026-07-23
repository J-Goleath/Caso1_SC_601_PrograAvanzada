using Caso1.Common;
using Caso1.Models.Account;
using Caso1.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Caso1.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                       HttpContext.GetOwinContext()
                           .GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ??
                       HttpContext.GetOwinContext()
                           .Get<ApplicationSignInManager>();
            }

            private set
            {
                _signInManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext
                    .GetOwinContext()
                    .Authentication;
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Nombre = model.Nombre,
                Apellidos = model.Apellidos,
                Activo = true
            };

            var result = await UserManager.CreateAsync(
                user,
                model.Password);

            if (result.Succeeded)
            {

                await UserManager.AddToRoleAsync(user.Id, Roles.Usuario);

                await SignInManager.SignInAsync(
                    user,
                    isPersistent: false,
                    rememberBrowser: false);

                return RedirectToAction(
                    "Listado",
                    "Tareas");
            }

            AddErrors(result);

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(
            LoginViewModel model,
            string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result =
                await SignInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    ModelState.AddModelError(
                        string.Empty,
                        "La cuenta está temporalmente bloqueada.");
                    break;

                case SignInStatus.RequiresVerification:
                    ModelState.AddModelError(
                        string.Empty,
                        "La cuenta requiere verificación.");
                    break;

                default:
                    ModelState.AddModelError(
                        string.Empty,
                        "Correo electrónico o contraseña incorrectos.");
                    break;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(
                DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction(
                "Login",
                "Account");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(
                "Listado",
                "Tareas");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager?.Dispose();
                _signInManager?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
