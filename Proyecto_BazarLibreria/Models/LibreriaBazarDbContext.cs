using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Proyecto_BazarLibreria.Models
{
    public class LibreriaBazarDbContext : DbContext
    {

        public LibreriaBazarDbContext() : base("LibreriaBazarConexion") { }

        public DbSet<CarritoItem> CarritoItems { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        //public DbSet<Carrito> Carritos { get; set; }
        //public DbSet<CarritoProducto> CarritoProductos { get; set; }
        public DbSet<Historial> Historiales { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Reseña> Reseñas { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }

       

    }
}