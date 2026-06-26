using Caso1.Entities;
using System.Collections.Generic;

namespace Caso1.infraestructure.Repositories
{
    public interface IPrioridadRepository : IRepository<Prioridad>
    {
        IEnumerable<Prioridad> GetPrioridadesActivas();
        bool ExistePrioridadConNombre(string nombre);
    }
}