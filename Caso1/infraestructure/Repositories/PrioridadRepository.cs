using Caso1.Entities;
using Caso1.infraestructure.DBContexts;
using System.Collections.Generic;
using System.Linq;

namespace Caso1.infraestructure.Repositories
{
    public class PrioridadRepository : Repository<Prioridad>, IPrioridadRepository
    {
        public PrioridadRepository(Caso1Context context) : base(context)
        {
        }

        public IEnumerable<Prioridad> GetPrioridadesActivas()
        {
            return Context.Prioridades.Where(p => !p.Borrado).ToList();
        }

        public bool ExistePrioridadConNombre(string nombre)
        {
            return Context.Prioridades.Any(p =>
                p.Nombre.Equals(nombre, System.StringComparison.OrdinalIgnoreCase) &&
                !p.Borrado);
        }
    }
}