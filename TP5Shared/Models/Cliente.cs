using System.Text.Json.Serialization;

namespace TP5Shared.Models
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

        [JsonIgnore]
        public ICollection<SalidaProducto> SalidasProductos { get; set; } = new List<SalidaProducto>();

    }
}
