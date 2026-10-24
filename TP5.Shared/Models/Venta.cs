namespace TP5.Shared.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }

        // Relación con Cliente
        public Cliente Cliente { get; set; }

        // Relación 1 a N con DetalleVenta
        public ICollection<DetalleVenta> Detalles { get; set; }
    }
}
