using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_BazarLibreria.Models
{
    public class Historial
    {
        public int Id { get; set; } // PK

        // AspNetUsers
        public string UsuarioId { get; set; } // FK a AspNetUsers

        // Relación con pedidos
        public ICollection<Pedido> Pedidos { get; set; }
    }


}