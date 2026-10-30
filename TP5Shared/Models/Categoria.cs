using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TP5Shared.Models
{
    /// <summary>
    /// Categorías de los productos.
    /// Una categoría puede tener muchos productos.
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// Clave primaria.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la categoría.
        /// Limitar a 100 caracteres evita datos excesivos
        /// y genera una columna más controlada en SQL Server.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Relación 1 a N:
        /// una categoría puede tener múltiples productos.
        /// JsonIgnore evita ciclos de serialización.
        /// </summary>
        [JsonIgnore]
        public ICollection<Producto> Productos
        {
            get; set;
        } = new List<Producto>();
    }
}