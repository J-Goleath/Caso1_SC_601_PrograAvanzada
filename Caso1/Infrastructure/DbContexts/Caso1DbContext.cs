using Caso1.Models.Entities;
using Caso1.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Caso1.Infrastructure.DbContexts
{

    public class Caso1DbContext : IdentityDbContext<ApplicationUser>
    {
        public Caso1DbContext() : base("name=Caso1DB")
        {
        }


        public static Caso1DbContext Create()
        {
            return new Caso1DbContext();
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Estado> Estados { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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


            modelBuilder.Entity<Tarea>()
                .HasOptional(t => t.Usuario)
                .WithMany()
                .HasForeignKey(t => t.UsuarioId);
        }
    }
}
