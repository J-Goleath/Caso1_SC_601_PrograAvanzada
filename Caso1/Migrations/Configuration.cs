namespace Caso1.Migrations
{
    using Caso1.Common;
    using Caso1.Models.Entities;
    using Caso1.Models.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Caso1.Infrastructure.DbContexts.Caso1DbContext>
    {
        // Credenciales del administrador inicial. Se crea únicamente si
        // todavía no existe (mismo patrón que CreateAdministrator en la
        // Lección 10). En un ambiente real esto se movería a
        // Web.config / variables de ambiente en vez de dejarlo fijo aquí.
        private const string AdminEmail = "admin@caso1.com";
        private const string AdminPassword = "Admin123*";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Caso1.Infrastructure.DbContexts.Caso1DbContext context)
        {
            context.Estados.AddOrUpdate(e => e.Nombre,
                new Estado { Nombre = "Pendiente", Descripcion = "Tarea aún no iniciada.", Orden = 1, Activo = true },
                new Estado { Nombre = "En Proceso", Descripcion = "Tarea en ejecución.", Orden = 2, Activo = true },
                new Estado { Nombre = "Completada", Descripcion = "Tarea finalizada con éxito.", Orden = 3, Activo = true },
                new Estado { Nombre = "Cancelada", Descripcion = "Tarea cancelada sin completar.", Orden = 4, Activo = true }
            );

            // Tarea investigativa asignada en clase (Lección 10 / Sprint 10,
            // Persona 1): crear los roles automáticamente mediante
            // Entity Framework Migrations y dejar un usuario Administrador
            // inicial ya sembrado en la base de datos ("data seeding").
            CrearRoles(context);
            CrearAdministradorInicial(context);
        }

        private static void CrearRoles(
            Caso1.Infrastructure.DbContexts.Caso1DbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            foreach (var nombreRol in Roles.Todos)
            {
                if (!roleManager.RoleExists(nombreRol))
                {
                    var resultado = roleManager.Create(
                        new IdentityRole(nombreRol));

                    if (!resultado.Succeeded)
                    {
                        throw new Exception(
                            string.Join(", ", resultado.Errors));
                    }
                }
            }
        }

        private static void CrearAdministradorInicial(
            Caso1.Infrastructure.DbContexts.Caso1DbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var admin = userManager.FindByEmail(AdminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    EmailConfirmed = true,
                    Nombre = "Administrador",
                    Apellidos = "del Sistema",
                    Activo = true
                };

                var resultadoCreacion = userManager.Create(
                    admin,
                    AdminPassword);

                if (!resultadoCreacion.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", resultadoCreacion.Errors));
                }
            }

            if (!userManager.IsInRole(admin.Id, Roles.Administrador))
            {
                var resultadoRol = userManager.AddToRole(
                    admin.Id,
                    Roles.Administrador);

                if (!resultadoRol.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", resultadoRol.Errors));
                }
            }
        }
    }
}