using Caso1.Infrastructure.Results;
using Caso1.Models.DTOs;
using Caso1.Models.Entities;
using System.Collections.Generic;
using System.Security.Principal;

namespace Caso1.Services
{
    public interface ITareaService
    {
        OperationResult<IEnumerable<Tarea>> Listar(string userId, IPrincipal user);

        OperationResult<Tarea> ObtenerPorId(int id, string userId, IPrincipal user);

        OperationResult Crear(CrearTareaDto dto, string userId);

        OperationResult Editar(EditarTareaDto dto, string userId, IPrincipal user);

        OperationResult Eliminar(int id, string userId, IPrincipal user);

        bool CanView(Tarea tarea, string userId, IPrincipal user);

        bool CanModify(Tarea tarea, string userId, IPrincipal user);
    }
}
