using System;
using TP5WebApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP5WebApi.Models
{
    // -------------------------
    // Clientes
    // -------------------------

    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string? Nombre { get; set; }

        [MaxLength(100)]
        public string? Contacto { get; set; }

        // Relación 1-N con SalidasProductos
        public ICollection<SalidaProducto> ?SalidasProductos { get; set; }
    }
}
