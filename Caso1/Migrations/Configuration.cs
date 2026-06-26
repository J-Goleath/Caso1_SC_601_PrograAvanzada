namespace Caso1.Migrations
{
    using Caso1.Entities;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Caso1.infraestructure.DBContexts.Caso1Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Caso1.infraestructure.DBContexts.Caso1Context context)
        {
            context.Estados.AddOrUpdate(e => e.Nombre,
                new Estado { Nombre = "Pendiente", Descripcion = "Tarea aún no iniciada.", Orden = 1, Activo = true },
                new Estado { Nombre = "En Proceso", Descripcion = "Tarea en ejecución.", Orden = 2, Activo = true },
                new Estado { Nombre = "Completada", Descripcion = "Tarea finalizada con éxito.", Orden = 3, Activo = true },
                new Estado { Nombre = "Cancelada", Descripcion = "Tarea cancelada sin completar.", Orden = 4, Activo = true }
            );
        }
    }
}