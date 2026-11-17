namespace TP5Shared.DTOs
{
    /// <summary>
    /// Representa una venta completa.
    ///
    /// Incluye cliente, fecha,
    /// total y detalle de productos vendidos.
    /// </summary>
    public class VentaDto
    {
        // Identificador único de la venta.
        public int Id { get; set; }

        // Cliente que realizó la compra.
        public int ClienteId { get; set; }

        // Nombre del cliente para mostrar en pantalla.
        public string ClienteNombre { get; set; } = string.Empty;

        // Fecha de la operación.
        public DateTime Fecha { get; set; }

        // Importe total de la venta.
        public decimal Total { get; set; }

        // Productos incluidos en la venta.
        public List<DetalleVentaDto> Detalles { get; set; } = new();
    }
}