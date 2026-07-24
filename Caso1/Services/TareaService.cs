using Caso1.Common;
using Caso1.Extensions;
using Caso1.Infrastructure.Repositories;
using Caso1.Infrastructure.Results;
using Caso1.Models.DTOs;
using Caso1.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;


namespace Caso1.Services
{
    public class TareaService : ITareaService
    {
        private readonly ITareaRepository _tareaRepository;

        public TareaService(ITareaRepository tareaRepository)
        {
            _tareaRepository = tareaRepository;
        }

        public bool CanView(Tarea tarea, string userId, IPrincipal user)
        {
            if (tarea == null) return false;

            // Admin ve todo
            if (user.IsInRole(Roles.Administrador)) return true;

            // El dueño siempre puede ver su propia tarea
            if (tarea.UsuarioId == userId) return true;

            // Compartida por rol: si la tarea tiene un rol asignado
            // y el usuario actual pertenece a ese rol
            if (!string.IsNullOrEmpty(tarea.RolCompartido) && user.IsInRole(tarea.RolCompartido))
                return true;

            return false;
        }

        public bool CanModify(Tarea tarea, string userId, IPrincipal user)
        {
            // Misma regla que CanView: propio, compartido por rol, o admin
            return CanView(tarea, userId, user);
        }

        public OperationResult<IEnumerable<Tarea>> Listar(string userId, IPrincipal user)
        {
            try
            {
                var tareas = _tareaRepository.Find(t => !t.Borrado)
                                              .Where(t => CanView(t, userId, user))
                                              .ToList();

                return OperationResult<IEnumerable<Tarea>>.Ok(tareas);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Tarea>>.Fail(
                    "Ocurrió un error al listar las tareas.",
                    new List<string> { ex.Message });
            }
        }

        public OperationResult<Tarea> ObtenerPorId(int id, string userId, IPrincipal user)
        {
            var tarea = _tareaRepository.GetById(id);

            if (tarea == null || tarea.Borrado)
                return OperationResult<Tarea>.Fail("La tarea no existe.");

            if (!CanView(tarea, userId, user))
                return OperationResult<Tarea>.Fail("No tiene permisos para ver esta tarea.");

            return OperationResult<Tarea>.Ok(tarea);
        }

        public OperationResult Crear(CrearTareaDto dto, string userId)
        {
            try
            {
                var tarea = new Tarea
                {
                    Titulo = dto.Titulo,
                    Detalle = dto.Detalle,
                    FechaHora = DateTime.Now,
                    Estado = EstadoTarea.Pendiente,
                    UsuarioId = userId
                };

                _tareaRepository.Add(tarea);
                return OperationResult.Ok("Tarea creada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(
                    "Ocurrió un error al crear la tarea.",
                    new List<string> { ex.Message });
            }
        }

        public OperationResult Editar(EditarTareaDto dto, string userId, IPrincipal user)
        {
            var tarea = _tareaRepository.GetById(dto.Id);

            if (tarea == null || tarea.Borrado)
                return OperationResult.Fail("La tarea no existe.");

            if (!CanModify(tarea, userId, user))
                return OperationResult.Fail("No tiene permisos para editar esta tarea.");

            try
            {
                tarea.Titulo = dto.Titulo;
                tarea.Detalle = dto.Detalle;
                tarea.Estado = dto.Estado.ToEstado();

                _tareaRepository.Update(tarea);
                return OperationResult.Ok("Tarea actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(
                    "Ocurrió un error al actualizar la tarea.",
                    new List<string> { ex.Message });
            }
        }

        public OperationResult Eliminar(int id, string userId, IPrincipal user)
        {
            var tarea = _tareaRepository.GetById(id);

            if (tarea == null || tarea.Borrado)
                return OperationResult.Fail("La tarea no existe.");

            if (!CanModify(tarea, userId, user))
                return OperationResult.Fail("No tiene permisos para eliminar esta tarea.");

            try
            {
                tarea.Borrado = true;
                _tareaRepository.Update(tarea);
                return OperationResult.Ok("Tarea eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(
                    "Ocurrió un error al eliminar la tarea.",
                    new List<string> { ex.Message });
            }
        }
    }
}