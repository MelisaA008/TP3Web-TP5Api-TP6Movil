using System;
using TP5.Shared.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP5.Shared.Models
{
    // -------------------------
    // Categorías
    // -------------------------
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string? Nombre { get; set; }

        // Relación 1-N con Productos
        public ICollection<Producto> ?Productos { get; set; }
    }
}