using System.Text.Json.Serialization;

namespace TP5Shared.Models
{
    // -------------------------
    // Productos
    // -------------------------

    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

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
        [JsonIgnore]
        public ICollection<IngresoProducto> IngresosProductos { get; set; } = new List<IngresoProducto>();

        [JsonIgnore]
        public ICollection<SalidaProducto> SalidasProductos { get; set; } = new List<SalidaProducto>();
    }
}