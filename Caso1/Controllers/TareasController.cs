using Caso1.Entities;
using Caso1.infraestructure.DBContexts;
using Caso1.infraestructure.Repositories;
using Caso1.Logging;
using System;
using System.Web.Mvc;

namespace Caso1.Controllers
{
    public class TareasController : Controller
    {
        private readonly ITareaRepository _tareaRepository;

        public TareasController()
        {
            var context = new Caso1Context();
            _tareaRepository = new TareaRepository(context);
        }

        public ActionResult Index()
        {
            //Error de prueba
            //try
            //{
            //    throw new Exception("Error de prueba caso 5");
            //}
            //catch (Exception ex)
            //{
            //    AppLogger.Error(ex, "Error en Tareas Index: {Mensaje}", ex.Message);
            //    TempData["Error"] = "Ocurrió un error inesperado.";
            //    return RedirectToAction("Index", "Home");
            //}

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
            AppLogger.Info("Acceso a creación de tarea");
            CargarEstados();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                try
                {
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
            }
            CargarEstados(tarea.Estado);
            return View(tarea);
        }

        public ActionResult Edit(int id)
        {
            AppLogger.Info("Acceso a edición de tarea ID {Id}", id);
            var tarea = _tareaRepository.GetById(id);
            if (tarea == null || tarea.Borrado)
                return HttpNotFound();
            CargarEstados(tarea.Estado);
            return View(tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                try
                {
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
            }
            CargarEstados(tarea.Estado);
            return View(tarea);
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

        private void CargarEstados(EstadoTarea seleccionado = EstadoTarea.Pendiente)
        {
            ViewBag.Estados = new SelectList(new[]
            {
                new { Value = (int)EstadoTarea.Pendiente,  Text = "Pendiente" },
                new { Value = (int)EstadoTarea.EnProceso,  Text = "En Proceso" },
                new { Value = (int)EstadoTarea.Completada, Text = "Completada" },
                new { Value = (int)EstadoTarea.Cancelada,  Text = "Cancelada" }
            }, "Value", "Text", (int)seleccionado);
        }
    }
}