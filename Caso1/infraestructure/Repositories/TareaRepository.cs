using Caso1.Entities;
using Caso1.infraestructure.DBContexts;

namespace Caso1.infraestructure.Repositories
{
    public class TareaRepository : Repository<Tarea>, ITareaRepository
    {
        public TareaRepository(Caso1Context context) : base(context)
        {
        }
    }
}
