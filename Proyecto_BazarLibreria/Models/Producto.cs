using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_BazarLibreria.Models
{
    public class Producto
    {
        [Key]
        public int Codigo { get; set; } // PK
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public bool Disponibilidad { get; set; } // En stock o no
        public bool Estado { get; set; } // Activo/Inactivo

        // Relación con otras tablas
        public ICollection<Imagen> Imagenes { get; set; }
        public ICollection<Reseña> Reseñas { get; set; }
        public ICollection<CarritoItem> CarritoItems { get; set; }
    }
    public class Imagen
    {

        public int Id { get; set; } // PK
        public string Url { get; set; }
        public int ProductoCodigo { get; set; } // FK
        public Producto Producto { get; set; }
    }

    public class Reseña
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string Usuario { get; set; }
        public string Comentario { get; set; }
        public int Calificación { get; set; }  // Por ejemplo, una calificación de 1 a 5
        public DateTime Fecha { get; set; }

        public virtual Producto Producto { get; set; }
    }
}