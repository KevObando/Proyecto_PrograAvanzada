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
    public class HistorialController : Controller
    {
        private readonly LibreriaBazarDbContext _context;

        public HistorialController()
        {
            _context = new LibreriaBazarDbContext();
        }

        public ActionResult Index()
        {
            // Obtiene el ID del usuario autenticado
            var userId = User.Identity.GetUserId();

            // Obtiene el historial según el rol
            var historiales = User.IsInRole("Admin") ?
                _context.Historiales.Include(h => h.Pedidos).ToList() :
                _context.Historiales.Where(h => h.UsuarioId == userId).Include(h => h.Pedidos).ToList();

            return View(historiales);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Obtiene el ID del usuario autenticado
            var userId = User.Identity.GetUserId();

            // Busca el historial y verifica que pertenezca al usuario autenticado o que sea admin
            var historial = _context.Historiales
                .Include(h => h.Pedidos)
                .FirstOrDefault(h => h.Id == id && (User.IsInRole("Admin") || h.UsuarioId == userId));

            if (historial == null)
                return HttpNotFound();

            return View(historial);
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
