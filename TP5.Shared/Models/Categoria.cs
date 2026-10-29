using System.Text.Json.Serialization;

namespace TP5.Shared.Models
{
    // -------------------------
    // Categorías
    // -------------------------
    using System.Text.Json.Serialization;

    public class Categoria
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Producto> Productos
        { get; set; }
            = new List<Producto>();
    }
}