using Caso1.Common;
using Caso1.Extensions;
using Caso1.Filters;
using Caso1.Infrastructure.DbContexts;
using Caso1.Infrastructure.Repositories;
using Caso1.Logging;
using Caso1.Models.DTOs;
using Caso1.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace Caso1.Controllers
{
    [Authorize]
    [CustomAuthenticationFilter]
    [ApplicationInfoResultFilter]
    public class TareasController : BaseController
    {
        private readonly ITareaService _tareaService;

        public TareasController()
        {
            var context = new Caso1DbContext();
            var tareaRepository = new TareaRepository(context);
            _tareaService = new TareaService(tareaRepository);
        }

        [ActionName("Listado")]
        public ActionResult Index()
        {
            AppLogger.Info("Acceso al listado de tareas");
            var resultado = _tareaService.Listar(User.Identity.GetUserId(), User);
            return View("Index", resultado.Data);
        }

        public ActionResult Details(int id)
        {
            AppLogger.Info("Acceso al detalle de la tarea ID {Id}", id);
            var resultado = _tareaService.ObtenerPorId(id, User.Identity.GetUserId(), User);

            if (!resultado.Success)
                return HttpNotFound();

            return View(resultado.Data);
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
            if (!ValidateDto(dto, new Validators.CrearTareaValidator()))
            {
                return View(dto);
            }

            var resultado = _tareaService.Crear(dto, User.Identity.GetUserId());

            if (resultado.Success)
            {
                AppLogger.Info("Tarea creada correctamente: {Titulo}", dto.Titulo);
                TempData["Exito"] = resultado.Message;
                return RedirectToAction("Listado");
            }

            AppLogger.Error(null, "Error al crear la tarea: {Mensaje}", resultado.Message);
            TempData["Error"] = resultado.Message;
            return View(dto);
        }

        public ActionResult Edit(int id)
        {
            AppLogger.Info("Acceso a edicion de tarea ID {Id}", id);
            var resultado = _tareaService.ObtenerPorId(id, User.Identity.GetUserId(), User);

            if (!resultado.Success)
                return HttpNotFound();

            var tarea = resultado.Data;
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
            if (!ValidateDto(dto, new Validators.EditarTareaValidator()))
            {
                return View(dto);
            }

            var resultado = _tareaService.Editar(dto, User.Identity.GetUserId(), User);

            if (resultado.Success)
            {
                AppLogger.Info("Tarea actualizada correctamente: {Titulo}", dto.Titulo);
                TempData["Exito"] = resultado.Message;
                return RedirectToAction("Listado");
            }

            AppLogger.Error(null, "Error al actualizar la tarea: {Mensaje}", resultado.Message);
            TempData["Error"] = resultado.Message;
            return View(dto);
        }

        public ActionResult Delete(int id)
        {
            AppLogger.Info("Acceso a eliminación de tarea ID {Id}", id);
            var resultado = _tareaService.ObtenerPorId(id, User.Identity.GetUserId(), User);

            if (!resultado.Success)
                return HttpNotFound();

            return View(resultado.Data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var resultado = _tareaService.Eliminar(id, User.Identity.GetUserId(), User);

            if (resultado.Success)
            {
                AppLogger.Info("Tarea eliminada correctamente ID {Id}", id);
                TempData["Exito"] = resultado.Message;
            }
            else
            {
                AppLogger.Error(null, "Error al eliminar la tarea ID {Id}: {Mensaje}", id, resultado.Message);
                TempData["Error"] = resultado.Message;
            }

            return RedirectToAction("Listado");
        }
    }
}