using System;
using TP5WebApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP5WebApi.Models
{
    // -------------------------
    // Productos
    // -------------------------

    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string? Nombre { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public string? ImagenPath { get; set; }

        // FK a Categoría
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        // FK a Proveedor
        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        // Relación con Ingresos y Salidas
        public ICollection<IngresoProducto> ?IngresosProductos { get; set; }
        public ICollection<SalidaProducto> ?SalidasProductos { get; set; }
    }
}