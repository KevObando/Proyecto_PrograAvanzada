using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Proyecto_BazarLibreria.Models;

namespace Proyecto_BazarLibreria.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private readonly LibreriaBazarDbContext _context;

        public ProductoController()
        {
            _context = new LibreriaBazarDbContext();
        }

        // Listado de productos: Disponible para todos los usuarios autenticados
        public ActionResult Index()
        {
            var productos = _context.Productos.Include(p => p.Imagenes).Include(p => p.Reseñas).ToList();
            return View(productos);
        }

        // Detalles de un producto: Disponible para todos los usuarios autenticados
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = _context.Productos
                .Include(p => p.Imagenes)
                .Include(p => p.Reseñas)
                .FirstOrDefault(p => p.Codigo == id);

            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        // Crear un nuevo producto: Disponible solo para Administradores
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // Editar un producto: Disponible solo para Administradores
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = _context.Productos.Find(id);
            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(producto).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // Eliminar un producto: Disponible solo para Administradores
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = _context.Productos.Find(id);
            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return HttpNotFound();

            _context.Productos.Remove(producto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}