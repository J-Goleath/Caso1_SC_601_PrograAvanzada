using Caso1.Models.Entities;
using Caso1.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caso1.Infrastructure.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(Caso1DbContext context) : base(context)
        {
        }

        public IEnumerable<Categoria> GetCategoriasActivas()
        {
            return Context.Categorias.Where(c => !c.Borrado).ToList();
        }

        public bool ExisteCategoriaConNombre(string nombre)
        {
            return Context.Categorias.Any(c => c.Nombre.Equals(nombre, System.StringComparison.OrdinalIgnoreCase) && !c.Borrado);
        }
    }
}