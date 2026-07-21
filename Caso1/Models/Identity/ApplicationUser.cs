using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Caso1.Models.Identity
{
    // Hereda de IdentityUser: por eso ya trae Id, UserName, Email,
    // PasswordHash, SecurityStamp, PhoneNumber, LockoutEnabled, etc.
    // Aquí solo agregamos los campos propios de nuestro dominio.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        public bool Activo { get; set; } = true;

        // Genera el "atestado" (claim) con el nombre completo del usuario
        // y arma la identidad que se guarda en la cookie de autenticación.
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(
                this,
                DefaultAuthenticationTypes.ApplicationCookie);

            userIdentity.AddClaim(
                new Claim("NombreCompleto", $"{Nombre} {Apellidos}"));

            return userIdentity;
        }
    }
}
