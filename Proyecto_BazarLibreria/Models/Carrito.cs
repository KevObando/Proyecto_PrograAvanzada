using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_BazarLibreria.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaAgregado { get; set; }

        // Relación con el modelo Producto
        public virtual Producto Producto { get; set; }
    }
}