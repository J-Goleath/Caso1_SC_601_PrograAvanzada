using Caso1.Models.Entities;
using Caso1.Infrastructure.DbContexts;

namespace Caso1.Infrastructure.Repositories
{
    public class TareaRepository : Repository<Tarea>, ITareaRepository
    {
        public TareaRepository(Caso1DbContext context) : base(context)
        {
        }
    }
}
