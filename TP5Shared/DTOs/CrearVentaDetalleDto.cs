namespace TP5Shared.DTOs
{
    /// <summary>
    /// Representa una línea de detalle al crear una venta.
    ///
    /// Indica qué producto se vende
    /// y cuántas unidades se descuentan del stock.
    /// </summary>
    public class CrearVentaDetalleDto
    {
        // Producto incluido en la venta.
        public int ProductoId { get; set; }

        // Cantidad de unidades vendidas.
        //
        // Ejemplo:
        // ProductoId = 5
        // Cantidad = 3
        //
        // Significa vender 3 unidades
        // del producto con Id 5.
        public int Cantidad { get; set; }
    }
}