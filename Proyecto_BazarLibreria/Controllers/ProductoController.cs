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
        public ActionResult Create(Producto producto, HttpPostedFileBase imagen)
        {
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    // Manejo de imagen
                    string carpetaImagenes = Server.MapPath("~/imagenes");
                    if (!Directory.Exists(carpetaImagenes))
                    {
                        Directory.CreateDirectory(carpetaImagenes);
                    }

                    string nombreArchivo = Path.GetFileName(imagen.FileName);
                    string rutaCompleta = Path.Combine(carpetaImagenes, nombreArchivo);
                    imagen.SaveAs(rutaCompleta);

                    var nuevaImagen = new Imagen
                    {
                        Url = "~/imagenes/" + nombreArchivo,
                        ProductoCodigo = producto.Codigo
                    };

                    producto.Imagenes = new List<Imagen> { nuevaImagen };
                }

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

            var producto = _context.Productos.Include(p => p.Imagenes).FirstOrDefault(p => p.Codigo == id);
            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Producto producto, HttpPostedFileBase imagen)
        {
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    // Manejo de imagen
                    string carpetaImagenes = Server.MapPath("~/imagenes");
                    if (!Directory.Exists(carpetaImagenes))
                    {
                        Directory.CreateDirectory(carpetaImagenes);
                    }

                    string nombreArchivo = Path.GetFileName(imagen.FileName);
                    string rutaCompleta = Path.Combine(carpetaImagenes, nombreArchivo);
                    imagen.SaveAs(rutaCompleta);

                    var nuevaImagen = new Imagen
                    {
                        Url = "~/imagenes/" + nombreArchivo,
                        ProductoCodigo = producto.Codigo
                    };

                    _context.Imagenes.Add(nuevaImagen);
                }

                _context.Entry(producto).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // Eliminar una imagen asociada a un producto
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EliminarImagen(int imagenId)
        {
            var imagen = _context.Imagenes.Find(imagenId);
            if (imagen == null)
            {
                return HttpNotFound();
            }

            var rutaImagen = Server.MapPath(imagen.Url);
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            _context.Imagenes.Remove(imagen);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = imagen.ProductoCodigo });
        }

        // Eliminar un producto: Disponible solo para Administradores
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = _context.Productos.Include(p => p.Imagenes).FirstOrDefault(p => p.Codigo == id);
            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var producto = _context.Productos.Include(p => p.Imagenes).FirstOrDefault(p => p.Codigo == id);
            if (producto == null) return HttpNotFound();

            foreach (var imagen in producto.Imagenes)
            {
                var rutaImagen = Server.MapPath(imagen.Url);
                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }
            }

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