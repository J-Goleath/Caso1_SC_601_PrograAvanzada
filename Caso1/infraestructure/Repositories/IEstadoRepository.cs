using System.Collections.Generic;
using Caso1.Entities;

namespace Caso1.infraestructure.Repositories
{
    public interface IEstadoRepository
    {
        IEnumerable<Estado> ObtenerTodos();
        Estado ObtenerPorId(int id);
        void Crear(Estado estado);
        void Actualizar(Estado estado);
        void Eliminar(int id);
        bool Existe(int id);
    }
}
