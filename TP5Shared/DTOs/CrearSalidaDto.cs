namespace TP5Shared.DTOs
{
    /// <summary>
    /// Datos necesarios para registrar una salida de stock.
    /// Se utiliza cuando un producto sale del inventario
    /// y queda asociado a un cliente.
    /// </summary>
    // Este DTO registra únicamente el movimiento de stock.
    // No almacena total, precio ni detalles de una venta completa.
    public class CrearSalidaDto
    {
        // Producto que sale del stock.
        public int ProductoId { get; set; }

        // Cliente relacionado con la salida.
        public int ClienteId { get; set; }

        // Cantidad de unidades que se descuentan.
        //
        // Ejemplo:
        // Stock actual = 20
        // Cantidad = 5
        //
        // Nuevo stock = 15
        public int Cantidad { get; set; }
    }
}