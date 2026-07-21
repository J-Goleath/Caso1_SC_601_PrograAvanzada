namespace Caso1.Common
{
    // Nombres de los roles del sistema. Se centralizan aquí para no
    // repetir "strings mágicos" en Configuration.Seed, controladores,
    // vistas y en los atributos [Authorize(Roles = "...")].
    //
    // Corresponden a los roles definidos en el Sprint 10 / Lección 10:
    // Admin, Supervisor, Soporte, Desarrollo, Usuario (aquí en español
    // para mantener la convención del resto del proyecto: Tareas,
    // Categorias, Prioridades, Estados, etc.).
    public static class Roles
    {
        public const string Administrador = "Administrador";
        public const string Supervisor = "Supervisor";
        public const string Soporte = "Soporte";
        public const string Desarrollo = "Desarrollo";
        public const string Usuario = "Usuario";

        public static readonly string[] Todos =
        {
            Administrador,
            Supervisor,
            Soporte,
            Desarrollo,
            Usuario
        };
    }
}
