using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto_BazarLibreria.Models;

namespace Proyecto_BazarLibreria.Controllers
{
    public class ProductoController : Controller
    {
        private LibreriaBazarDbContext db = new LibreriaBazarDbContext();

        // GET: Producto
        public ActionResult Index()
        {
            return View(db.Productos.ToList());
        }

        // GET: Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var producto = db.Productos
                             .Include(p => p.Imagenes) // Incluye las imágenes asociadas
                             .FirstOrDefault(p => p.Codigo == id);

            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo,Nombre,Precio,Disponibilidad,Estado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Producto producto = db.Productos.Include(p => p.Imagenes).FirstOrDefault(p => p.Codigo == id);

            if (producto == null)
            {
                return HttpNotFound();
            }

            return View(producto);
        }

        // POST: Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Producto producto, HttpPostedFileBase imagen)
        {
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    // Ruta donde se guardarán las imágenes
                    string carpetaImagenes = Server.MapPath("~/imagenes");

                    // Verifica si el directorio existe, y si no, lo crea
                    if (!Directory.Exists(carpetaImagenes))
                    {
                        Directory.CreateDirectory(carpetaImagenes);
                    }

                    // Construye la ruta completa
                    string rutaCompleta = Path.Combine(carpetaImagenes, Path.GetFileName(imagen.FileName));

                    // Guarda la imagen en el servidor
                    imagen.SaveAs(rutaCompleta);

                    // Asocia la imagen con el producto
                    var nuevaImagen = new Imagen
                    {
                        Url = "~/imagenes/" + imagen.FileName,
                        ProductoCodigo = producto.Codigo
                    };

                    // Agrega la imagen al producto
                    db.Imagenes.Add(nuevaImagen);
                }

                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EliminarImagen(int imagenId)
        {
            // Busca la imagen en la base de datos
            var imagen = db.Imagenes.Find(imagenId);
            if (imagen == null)
            {
                return HttpNotFound();
            }

            // Elimina el archivo físico si existe
            var rutaImagen = Server.MapPath(imagen.Url);
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            // Elimina la imagen de la base de datos
            db.Imagenes.Remove(imagen);
            db.SaveChanges();

            // Redirige al detalle del producto después de eliminar la imagen
            return RedirectToAction("Details", new { id = imagen.ProductoCodigo });
        }

        // POST: Producto/AgregarReseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarReseña(int productoId, string comentario, int rating, string usuario)
        {
            if (ModelState.IsValid)
            {
                // Encuentra el producto por su ID
                var producto = db.Productos.FirstOrDefault(p => p.Codigo == productoId);
                if (producto != null)
                {
                    // Crea una nueva reseña
                    var nuevaReseña = new Reseña
                    {
                        Comentario = comentario,
                        Calificación = rating,
                        Fecha = DateTime.Now,
                        ProductoId = productoId,
                        Usuario = usuario // Guardamos el nombre de usuario
                    };

                    // Agrega la reseña a la base de datos
                    db.Reseñas.Add(nuevaReseña);
                    db.SaveChanges();

                    // Redirige al detalle del producto
                    return RedirectToAction("Details", new { id = productoId });
                }
            }

            // Si algo sale mal, vuelve a la vista de detalles del producto
            return RedirectToAction("Details", new { id = productoId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}