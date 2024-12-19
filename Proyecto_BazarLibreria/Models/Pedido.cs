using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_BazarLibreria.Models
{
    public class Pedido
    {
        public int Id { get; set; } // PK

        // Ajuste para utilizar el UserId de Identity
        public string UsuarioId { get; set; } // FK a AspNetUsers

        // Relación con el historial (si aplica)
        public int? HistorialId { get; set; } // FK opcional para relación con Historial
        public Historial Historial { get; set; }

        // Propiedades adicionales
        public DateTime Fecha { get; set; } // Fecha de creación del pedido
        public decimal Total { get; set; } // Total del pedido
    }
}