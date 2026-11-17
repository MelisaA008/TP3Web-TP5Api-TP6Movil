namespace TP5Shared.DTOs
{
    /// <summary>
    /// Datos necesarios para registrar una venta.
    ///
    /// Incluye el cliente y la lista de productos
    /// que forman parte de la operación.
    /// </summary>
    public class CrearVentaDto
    {
        // Cliente que realiza la compra.
        public int ClienteId { get; set; }

        // Productos incluidos en la venta.
        //
        // Cada detalle indica:
        // - qué producto se vende
        // - cuántas unidades se descuentan del stock
        public List<CrearVentaDetalleDto> Detalles { get; set; } = new();
    }
}