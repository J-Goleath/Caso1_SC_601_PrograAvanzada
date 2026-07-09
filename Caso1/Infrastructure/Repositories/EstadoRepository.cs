using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Caso1.Models.Entities;
using Caso1.Infrastructure.DbContexts;

namespace Caso1.Infrastructure.Repositories
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly Caso1DbContext _context;

        public EstadoRepository()
        {
            _context = new Caso1DbContext();
        }

        public IEnumerable<Estado> ObtenerTodos()
        {
            return _context.Estados
                           .OrderBy(e => e.Orden)
                           .ToList();
        }

        public Estado ObtenerPorId(int id)
        {
            return _context.Estados.Find(id);
        }

        public void Crear(Estado estado)
        {
            _context.Estados.Add(estado);
            _context.SaveChanges();
        }

        public void Actualizar(Estado estado)
        {
            _context.Entry(estado).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var estado = _context.Estados.Find(id);
            if (estado != null)
            {
                _context.Estados.Remove(estado);
                _context.SaveChanges();
            }
        }

        public bool Existe(int id)
        {
            return _context.Estados.Any(e => e.EstadoId == id);
        }
    }
}