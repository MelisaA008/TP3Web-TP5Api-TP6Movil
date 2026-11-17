using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using TP5Shared.Models;

namespace TP5Shared.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }

        public int VentaId { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        [JsonIgnore]
        public Venta? Venta { get; set; }

        [JsonIgnore]
        public Producto? Producto { get; set; }
    }
}
