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
        public ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();
        public ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
        public ICollection<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();
    }
    public class Imagen
    {
        public int Id { get; set; } // PK
        public string Url { get; set; }

        // Relación con Producto
        public int ProductoCodigo { get; set; } // FK
        public Producto Producto { get; set; }

    }

    public class Reseña

    {
        public int Id { get; set; } // PK

        // Relación con Producto
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        // Relación con Usuario
        public string UsuarioId { get; set; } // FK a AspNetUsers

        // Propiedades adicionales
        public string Comentario { get; set; }
        public int Calificacion { get; set; } // Por ejemplo, una calificación de 1 a 5
        public DateTime Fecha { get; set; }
    }

}