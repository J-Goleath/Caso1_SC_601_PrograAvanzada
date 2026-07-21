using Caso1.Infrastructure.DbContexts;
using Caso1.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace Caso1
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Un contexto de base de datos, un user manager y un
            // signin manager por cada request (patrón per-owin-context).
            app.CreatePerOwinContext(Caso1DbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(
                ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(
                ApplicationSignInManager.Create);

            // Configuración de la cookie de autenticación.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieName = "Caso1.Tareas.Cookie",
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity =
                        SecurityStampValidator
                            .OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                                validateInterval: TimeSpan.FromMinutes(30),
                                regenerateIdentity: (manager, user) =>
                                    user.GenerateUserIdentityAsync(manager))
                }
            });
        }
    }
}
