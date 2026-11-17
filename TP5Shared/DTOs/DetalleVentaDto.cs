namespace TP5Shared.DTOs
{
    /// <summary>
    /// Representa un producto dentro de una venta.
    // Una venta puede tener varios detalles.
    // Cada detalle representa un producto diferente.
    // Cada detalle genera una salida de stock según la cantidad vendida.
    // Guarda la cantidad vendida, el precio aplicado y el subtotal generado.
    /// </summary>
    public class DetalleVentaDto
    {
        // Producto incluido en la venta.
        public int ProductoId { get; set; }

        // Nombre del producto para mostrar en pantalla,
        // tickets y reportes.
        public string ProductoNombre { get; set; } = string.Empty;

        // Cantidad de unidades vendidas.
        public int Cantidad { get; set; }

        // Precio unitario al momento de la venta.
        //
        // Se guarda para conservar el valor histórico,
        // aunque el precio del producto cambie después.
        public decimal PrecioUnitario { get; set; }

        // Importe total de esta línea.
        //
        // Ejemplo:
        // 3 unidades × $100 = $300
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}