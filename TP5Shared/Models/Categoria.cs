using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TP5Shared.Models
{
    /// Categorías de los productos.
     public class Categoria
    {
        // Clave primaria.
        public int Id { get; set; }

        // Nombre de la categoría.
        // Limitar a 100 caracteres evita datos excesivos y genera una columna más controlada en SQL Server.
       
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        // Relación 1 a N: una categoría puede tener muchos productos.
        // JsonIgnore evita ciclos de serialización.
       
        [JsonIgnore]
        public ICollection<Producto> Productos
        {
            get; set;
        } = new List<Producto>();
    }
}