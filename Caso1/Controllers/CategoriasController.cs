using Caso1.Models.DTOs;
using Caso1.Models.Entities;
using Caso1.Infrastructure.DbContexts;
using Caso1.Infrastructure.Repositories;
using Caso1.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caso1.Controllers
{
    [RoutePrefix("Categorias")]
    public class CategoriasController : BaseController
    {
        private readonly ICategoriaRepository _repository;

        public CategoriasController()
        {
            var context = new Caso1DbContext();
            _repository = new CategoriaRepository(context);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var categorias = _repository.GetAll();
            return View(categorias);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CrearCategoriaDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearCategoriaDto dto)
        {
            if (!ValidateDto(dto, new CrearCategoriaValidator()))
            {
                return View(dto);
            }

            if (_repository.ExisteCategoriaConNombre(dto.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe una categoria con este nombre");
                return View(dto);
            }

            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                FechaCreacion = DateTime.Now
            };

            _repository.Add(categoria);
            TempData["MensajeExito"] = "Categoria registrada correctamente";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = _repository.GetById(id);
            if (model == null || model.Borrado)
            {
                TempData["MensajeError"] = "La categoria no existe";
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
                TempData["MensajeError"] = "La categoria no existe";
                return RedirectToAction(nameof(Index));
            }

            var dto = new EditarCategoriaDto
            {
                Id = model.Id,
                Nombre = model.Nombre,
                Descripcion = model.Descripcion
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditarCategoriaDto dto)
        {
            if (!ValidateDto(dto, new EditarCategoriaValidator()))
            {
                return View(dto);
            }

            var categoriaExistente = _repository.GetById(dto.Id);

            if (categoriaExistente == null || categoriaExistente.Borrado)
            {
                TempData["MensajeError"] = "La categoria no existe";
                return RedirectToAction(nameof(Index));
            }

            var existe = _repository.ExisteCategoriaConNombre(dto.Nombre);

            if (existe && categoriaExistente.Nombre != dto.Nombre)
            {
                ModelState.AddModelError("Nombre", "Ya existe otra categoria con este nombre");
                return View(dto);
            }

            categoriaExistente.Nombre = dto.Nombre;
            categoriaExistente.Descripcion = dto.Descripcion;

            _repository.Update(categoriaExistente);
            TempData["MensajeExito"] = "Categoria actualizada correctamente";
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
                TempData["MensajeExito"] = "Categoria eliminada correctamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["MensajeError"] = "La categoria no existe";
            return RedirectToAction(nameof(Index));
        }
    }
}
