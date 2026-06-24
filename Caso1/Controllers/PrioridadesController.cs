using Caso1.Entities;
using Caso1.infraestructure.DBContexts;
using Caso1.infraestructure.Repositories;
using System;
using System.Web.Mvc;

namespace Caso1.Controllers
{
    [RoutePrefix("Prioridades")]
    public class PrioridadesController : Controller
    {
        private readonly IPrioridadRepository _repository;

        public PrioridadesController()
        {
            var context = new Caso1Context();
            _repository = new PrioridadRepository(context);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var prioridades = _repository.GetPrioridadesActivas();
            return View(prioridades);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new Prioridad();
            model.Id = new Random().Next(1000000);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Prioridad prioridad)
        {
            if (ModelState.IsValid)
            {
                if (_repository.ExistePrioridadConNombre(prioridad.Nombre))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una prioridad con este nombre");
                    return View(prioridad);
                }

                prioridad.FechaCreacion = DateTime.Now;
                _repository.Add(prioridad);
                TempData["MensajeExito"] = "Prioridad registrada correctamente";
                return RedirectToAction(nameof(Index));
            }

            return View(prioridad);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = _repository.GetById(id);

            if (model == null || model.Borrado)
            {
                TempData["MensajeError"] = "La prioridad no existe";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _repository.GetById(id);

            if (model == null || model.Borrado)
            {
                TempData["MensajeError"] = "La prioridad no existe";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Prioridad prioridad)
        {
            if (ModelState.IsValid)
            {
                var existe = _repository.ExistePrioridadConNombre(prioridad.Nombre);
                var actual = _repository.GetById(prioridad.Id);

                if (existe && actual != null && actual.Nombre != prioridad.Nombre)
                {
                    ModelState.AddModelError("Nombre", "Ya existe otra prioridad con este nombre");
                    return View(prioridad);
                }

                var prioridadExistente = _repository.GetById(prioridad.Id);

                if (prioridadExistente != null)
                {
                    prioridad.FechaCreacion = prioridadExistente.FechaCreacion;
                    prioridad.Borrado = prioridadExistente.Borrado;
                }

                _repository.Update(prioridad);
                TempData["MensajeExito"] = "Prioridad actualizada correctamente";
                return RedirectToAction(nameof(Index));
            }

            return View(prioridad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var model = _repository.GetById(id);

            if (model != null && !model.Borrado)
            {
                model.Borrado = true;
                _repository.Update(model);
                TempData["MensajeExito"] = "Prioridad eliminada correctamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["MensajeError"] = "La prioridad no existe";
            return RedirectToAction(nameof(Index));
        }
    }
}