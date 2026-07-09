using System;
using System.Web.Mvc;
using Caso1.Models.DTOs;
using Caso1.Models.Entities;
using Caso1.Infrastructure.Repositories;
using Caso1.Logging;
using Caso1.Validators;

namespace Caso1.Controllers
{
    public class EstadosController : BaseController
    {
        private readonly IEstadoRepository _repo;

        public EstadosController()
        {
            _repo = new EstadoRepository();
        }

        public ActionResult Index()
        {
            var estados = _repo.ObtenerTodos();
            return View(estados);
        }

        public ActionResult Details(int id)
        {
            var estado = _repo.ObtenerPorId(id);
            if (estado == null)
            {
                TempData["Error"] = "Estado no encontrado.";
                return RedirectToAction("Index");
            }
            return View(estado);
        }

        public ActionResult Create()
        {
            return View(new CrearEstadoDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearEstadoDto dto)
        {
            if (!ValidateDto(dto, new CrearEstadoValidator()))
            {
                return View(dto);
            }

            try
            {
                var estado = new Estado
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Orden = dto.Orden,
                    Activo = true
                };

                _repo.Crear(estado);
                AppLogger.Info("Estado creado: {Nombre} (Orden: {Orden})",
                               estado.Nombre, estado.Orden);
                TempData["Exito"] = "Estado creado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Error al crear estado: {Nombre}", dto.Nombre);
                TempData["Error"] = "Ocurrió un error al crear el estado.";
                return View(dto);
            }
        }

        public ActionResult Edit(int id)
        {
            var estado = _repo.ObtenerPorId(id);
            if (estado == null)
            {
                TempData["Error"] = "Estado no encontrado.";
                return RedirectToAction("Index");
            }

            var dto = new EditarEstadoDto
            {
                EstadoId = estado.EstadoId,
                Nombre = estado.Nombre,
                Descripcion = estado.Descripcion,
                Orden = estado.Orden,
                Activo = estado.Activo
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditarEstadoDto dto)
        {
            if (!ValidateDto(dto, new EditarEstadoValidator()))
            {
                return View(dto);
            }

            try
            {
                var estado = _repo.ObtenerPorId(dto.EstadoId);
                if (estado == null)
                {
                    TempData["Error"] = "Estado no encontrado.";
                    return RedirectToAction("Index");
                }

                estado.Nombre = dto.Nombre;
                estado.Descripcion = dto.Descripcion;
                estado.Orden = dto.Orden;
                estado.Activo = dto.Activo;

                _repo.Actualizar(estado);
                AppLogger.Info("Estado actualizado: {Nombre} (Id: {Id})",
                               estado.Nombre, estado.EstadoId);
                TempData["Exito"] = "Estado actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Error al actualizar estado Id: {Id}", dto.EstadoId);
                TempData["Error"] = "Ocurrio un error al actualizar el estado.";
                return View(dto);
            }
        }

        public ActionResult Delete(int id)
        {
            var estado = _repo.ObtenerPorId(id);
            if (estado == null)
            {
                TempData["Error"] = "Estado no encontrado.";
                return RedirectToAction("Index");
            }
            return View(estado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var estado = _repo.ObtenerPorId(id);
                _repo.Eliminar(id);
                AppLogger.Info("Estado eliminado: {Nombre} (Id: {Id})",
                               estado?.Nombre, id);
                TempData["Exito"] = "Estado eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Error al eliminar estado Id: {Id}", id);
                TempData["Error"] = "Ocurrió un error al eliminar el estado.";
                return RedirectToAction("Index");
            }
        }
    }
}
