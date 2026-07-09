using Caso1.Models.DTOs;
using Caso1.Models.Entities;
using Caso1.Infrastructure.DbContexts;
using Caso1.Infrastructure.Repositories;
using Caso1.Validators;
using System;
using System.Web.Mvc;

namespace Caso1.Controllers
{
    [RoutePrefix("Prioridades")]
    public class PrioridadesController : BaseController
    {
        private readonly IPrioridadRepository _repository;

        public PrioridadesController()
        {
            var context = new Caso1DbContext();
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
            return View(new CrearPrioridadDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearPrioridadDto dto)
        {
            if (!ValidateDto(dto, new CrearPrioridadValidator()))
            {
                return View(dto);
            }

            if (_repository.ExistePrioridadConNombre(dto.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe una prioridad con este nombre");
                return View(dto);
            }

            var prioridad = new Prioridad
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                FechaCreacion = DateTime.Now
            };

            _repository.Add(prioridad);
            TempData["MensajeExito"] = "Prioridad registrada correctamente";
            return RedirectToAction(nameof(Index));
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

            var dto = new EditarPrioridadDto
            {
                Id = model.Id,
                Nombre = model.Nombre,
                Descripcion = model.Descripcion
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditarPrioridadDto dto)
        {
            if (!ValidateDto(dto, new EditarPrioridadValidator()))
            {
                return View(dto);
            }

            var prioridadExistente = _repository.GetById(dto.Id);

            if (prioridadExistente == null || prioridadExistente.Borrado)
            {
                TempData["MensajeError"] = "La prioridad no existe";
                return RedirectToAction(nameof(Index));
            }

            var existe = _repository.ExistePrioridadConNombre(dto.Nombre);

            if (existe && prioridadExistente.Nombre != dto.Nombre)
            {
                ModelState.AddModelError("Nombre", "Ya existe otra prioridad con este nombre");
                return View(dto);
            }

            prioridadExistente.Nombre = dto.Nombre;
            prioridadExistente.Descripcion = dto.Descripcion;

            _repository.Update(prioridadExistente);
            TempData["MensajeExito"] = "Prioridad actualizada correctamente";
            return RedirectToAction(nameof(Index));
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
