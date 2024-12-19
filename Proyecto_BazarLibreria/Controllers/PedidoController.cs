using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Proyecto_BazarLibreria.Models;

namespace Proyecto_BazarLibreria.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private readonly LibreriaBazarDbContext _context;

        public PedidoController()
        {
            _context = new LibreriaBazarDbContext();
        }

        // Listado de pedidos
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            // Mostrar todos los pedidos si es admin, solo los del usuario autenticado si no
            var pedidos = User.IsInRole("Admin")
                ? _context.Pedidos.Include("Historial").ToList()
                : _context.Pedidos.Where(p => p.UsuarioId == userId).Include("Historial").ToList();

            return View(pedidos);
        }

        // Detalles de un pedido
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userId = User.Identity.GetUserId();

            var pedido = _context.Pedidos
                .Include("Historial")
                .FirstOrDefault(p => p.Id == id && (User.IsInRole("Admin") || p.UsuarioId == userId));

            if (pedido == null) return HttpNotFound();

            return View(pedido);
        }

        // Crear un nuevo pedido
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                pedido.UsuarioId = User.Identity.GetUserId();
                pedido.Fecha = DateTime.Now;

                _context.Pedidos.Add(pedido);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(pedido);
        }

        // Editar un pedido
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userId = User.Identity.GetUserId();

            var pedido = _context.Pedidos.FirstOrDefault(p => p.Id == id && (User.IsInRole("Admin") || p.UsuarioId == userId));

            if (pedido == null) return HttpNotFound();

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                var pedidoDb = _context.Pedidos.FirstOrDefault(p => p.Id == pedido.Id);

                if (pedidoDb == null) return HttpNotFound();

                // Solo permitir editar ciertas propiedades
                pedidoDb.Total = pedido.Total;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(pedido);
        }

        // Eliminar un pedido
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userId = User.Identity.GetUserId();

            var pedido = _context.Pedidos.FirstOrDefault(p => p.Id == id && (User.IsInRole("Admin") || p.UsuarioId == userId));

            if (pedido == null) return HttpNotFound();

            return View(pedido);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pedido = _context.Pedidos.Find(id);

            if (pedido == null) return HttpNotFound();

            _context.Pedidos.Remove(pedido);
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
