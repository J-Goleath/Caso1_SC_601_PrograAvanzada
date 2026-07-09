using Caso1.Common;
using Caso1.Models.DTOs;
using Caso1.Models.Entities;
using Caso1.Extensions;
using Caso1.Infrastructure.DbContexts;
using Caso1.Infrastructure.Repositories;
using Caso1.Logging;
using Caso1.Validators;
using System;
using System.Web.Mvc;

namespace Caso1.Controllers
{
    public class TareasController : BaseController
    {
        private readonly ITareaRepository _tareaRepository;

        public TareasController()
        {
            var context = new Caso1DbContext();
            _tareaRepository = new TareaRepository(context);
        }

        public ActionResult Index()
        {
            AppLogger.Info("Acceso al listado de tareas");
            var tareas = _tareaRepository.Find(t => !t.Borrado);
            return View(tareas);
        }

        public ActionResult Details(int id)
        {
            AppLogger.Info("Acceso al detalle de la tarea ID {Id}", id);
            var tarea = _tareaRepository.GetById(id);
            if (tarea == null || tarea.Borrado)
                return HttpNotFound();
            return View(tarea);
        }

        public ActionResult Create()
        {
            AppLogger.Info("Acceso a creacion de tarea");
            return View(new CrearTareaDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearTareaDto dto)
        {
            if (!ValidateDto(dto, new CrearTareaValidator()))
            {
                return View(dto);
            }

            try
            {
                var tarea = new Tarea
                {
                    Titulo = dto.Titulo,
                    Detalle = dto.Detalle,
                    FechaHora = DateTime.Now,
                    Estado = EstadoTarea.Pendiente
                };

                _tareaRepository.Add(tarea);
                AppLogger.Info("Tarea creada correctamente: {Titulo}", tarea.Titulo);
                TempData["Exito"] = "Tarea creada correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Error al crear la tarea");
                TempData["Error"] = "Ocurrió un error al guardar la tarea.";
            }

            return View(dto);
        }

        public ActionResult Edit(int id)
        {
            AppLogger.Info("Acceso a edicion de tarea ID {Id}", id);
            var tarea = _tareaRepository.GetById(id);
            if (tarea == null || tarea.Borrado)
                return HttpNotFound();

            var dto = new EditarTareaDto
            {
                Id = tarea.Id,
                Titulo = tarea.Titulo,
                Detalle = tarea.Detalle,
                Estado = tarea.Estado.ToText()
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditarTareaDto dto)
        {
            if (!ValidateDto(dto, new EditarTareaValidator()))
            {
                return View(dto);
            }

            try
            {
                var tarea = _tareaRepository.GetById(dto.Id);
                if (tarea == null || tarea.Borrado)
                    return HttpNotFound();

                tarea.Titulo = dto.Titulo;
                tarea.Detalle = dto.Detalle;
                tarea.Estado = dto.Estado.ToEstado();

                _tareaRepository.Update(tarea);
                AppLogger.Info("Tarea actualizada correctamente: {Titulo}", tarea.Titulo);
                TempData["Exito"] = "Tarea actualizada correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Error al actualizar la tarea");
                TempData["Error"] = "Ocurrió un error al actualizar la tarea.";
            }

            return View(dto);
        }

        public ActionResult Delete(int id)
        {
            AppLogger.Info("Acceso a eliminación de tarea ID {Id}", id);
            var tarea = _tareaRepository.GetById(id);
            if (tarea == null || tarea.Borrado)
                return HttpNotFound();
            return View(tarea);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var tarea = _tareaRepository.GetById(id);
                if (tarea == null || tarea.Borrado)
                    return HttpNotFound();

                tarea.Borrado = true;
                _tareaRepository.Update(tarea);
                AppLogger.Info("Tarea eliminada correctamente ID {Id}", id);
                TempData["Exito"] = "Tarea eliminada correctamente.";
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Error al eliminar la tarea ID {Id}", id);
                TempData["Error"] = "Ocurrió un error al eliminar la tarea.";
            }
            return RedirectToAction("Index");
        }
    }
}
