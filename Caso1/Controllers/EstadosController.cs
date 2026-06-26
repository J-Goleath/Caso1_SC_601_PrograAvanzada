using System;
using System.Web.Mvc;
using Caso1.Entities;
using Caso1.infraestructure.Repositories;
using Caso1.Logging;

namespace Caso1.Controllers
{
    public class EstadosController : Controller
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Estado estado)
        {
            if (!ModelState.IsValid)
                return View(estado);

            try
            {
                _repo.Crear(estado);
                AppLogger.Info("Estado creado: {Nombre} (Orden: {Orden})",
                               estado.Nombre, estado.Orden);
                TempData["Exito"] = "Estado creado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Error al crear estado: {Nombre}", estado.Nombre);
                TempData["Error"] = "Ocurrió un error al crear el estado.";
                return View(estado);
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
            return View(estado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Estado estado)
        {
            if (!ModelState.IsValid)
                return View(estado);

            try
            {
                _repo.Actualizar(estado);
                AppLogger.Info("Estado actualizado: {Nombre} (Id: {Id})",
                               estado.Nombre, estado.EstadoId);
                TempData["Exito"] = "Estado actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Error al actualizar estado Id: {Id}", estado.EstadoId);
                TempData["Error"] = "Ocurrio un error al actualizar el estado.";
                return View(estado);
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
