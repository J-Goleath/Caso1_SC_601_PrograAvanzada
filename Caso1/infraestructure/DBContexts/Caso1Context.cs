using Caso1.Entities;
using System.Data.Entity;

namespace Caso1.infraestructure.DBContexts
{
    public class Caso1Context : DbContext
    {
        public Caso1Context() : base("name=Caso1DB")
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Estado> Estados { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().ToTable("Categorias");
            modelBuilder.Entity<Prioridad>().ToTable("Prioridades");
            modelBuilder.Entity<Tarea>().ToTable("Tareas");
            modelBuilder.Entity<Estado>().ToTable("Estados");

            modelBuilder.Entity<Categoria>()
                .Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Categoria>()
                .Property(c => c.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Prioridad>()
                .Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Prioridad>()
                .Property(p => p.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            base.OnModelCreating(modelBuilder);
        }
    }
}