using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_BazarLibreria.Models
{
    public class Usuario
    {
        [Key]
        public int Codigo { get; set; } // PK
        public string Nombre { get; set; }
        public string Contraseña { get; set; } // Encriptada
        public DateTime UltimaConexion { get; set; }
        public bool Estado { get; set; } // Activo/Inactivo

        // Relación con otras tablas
        public ICollection<CarritoItem> Carritos { get; set; }
        public ICollection<Historial> Historiales { get; set; }
    }
}