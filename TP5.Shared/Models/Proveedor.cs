using System.ComponentModel.DataAnnotations;

namespace TP5.Shared.Models
{
    // -------------------------
    // Proveedores
    // -------------------------
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string? Nombre { get; set; }

        [MaxLength(100)]
        public string? Contacto { get; set; }

        // Relación 1-N con Productos
        public ICollection<Producto> ?Productos { get; set; }
    }
}