using Caso1.Models.Entities;
using System.Collections.Generic;

namespace Caso1.Infrastructure.Repositories
{
    public interface IPrioridadRepository : IRepository<Prioridad>
    {
        IEnumerable<Prioridad> GetPrioridadesActivas();
        bool ExistePrioridadConNombre(string nombre);
    }
}