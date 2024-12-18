using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Proyecto_BazarLibreria.Models;


namespace Proyecto_BazarLibreria.Controllers
{
    [Authorize]
    public class CarritoController : Controller
    {
        private readonly LibreriaBazarDbContext _context;

        public CarritoController()
        {
            _context = new LibreriaBazarDbContext();
        }

        // Vista principal del carrito
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId(); // Obtiene el ID único del usuario autenticado

            // Consulta los elementos del carrito solo para el usuario autenticado
            var carrito = _context.CarritoItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Producto) // Incluye la relación con Producto para obtener los detalles
                .ToList();

            // Retorna la vista con el carrito
            return View(carrito);
        }

        // Agregar un producto al carrito
        [HttpPost]
        public ActionResult Agregar(int productoId, int cantidad = 1)
        {
            var userId = User.Identity.GetUserId();

            var itemExistente = _context.CarritoItems
                .FirstOrDefault(c => c.UserId == userId && c.ProductoId == productoId);

            if (itemExistente != null)
            {
                // Si el producto ya está en el carrito, aumenta la cantidad
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                // Agregar un nuevo producto al carrito
                var nuevoItem = new CarritoItem
                {
                    UserId = userId,
                    ProductoId = productoId,
                    Cantidad = cantidad,
                    FechaAgregado = DateTime.Now
                };
                _context.CarritoItems.Add(nuevoItem);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Eliminar un producto del carrito
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            var userId = User.Identity.GetUserId();

            var item = _context.CarritoItems.FirstOrDefault(c => c.Id == id && c.UserId == userId);
            if (item != null)
            {
                _context.CarritoItems.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Actualizar la cantidad de un producto en el carrito
        [HttpPost]
        public ActionResult ActualizarCantidad(int id, int cantidad)
        {
            var userId = User.Identity.GetUserId();

            var item = _context.CarritoItems.FirstOrDefault(c => c.Id == id && c.UserId == userId);
            if (item != null && cantidad > 0)
            {
                item.Cantidad = cantidad;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Vaciar el carrito
        [HttpPost]
        public ActionResult Vaciar()
        {
            var userId = User.Identity.GetUserId();

            var items = _context.CarritoItems.Where(c => c.UserId == userId).ToList();
            if (items.Any())
            {
                _context.CarritoItems.RemoveRange(items);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}






