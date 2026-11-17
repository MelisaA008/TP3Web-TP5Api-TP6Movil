namespace TP5Shared.DTOs
{
    /// <summary>
    /// Resumen de una venta.
    ///
    /// Se usa para mostrar listados,
    /// reportes y consultas rápidas
    /// sin necesidad de cargar todos los detalles.
    /// </summary>
    public class VentaListaDto
    {
        // Identificador único de la venta.
        public int Id { get; set; }

        // Cliente que realizó la compra.
        public string ClienteNombre { get; set; } = string.Empty;

        // Fecha en la que se registró la venta.
        public DateTime Fecha { get; set; }

        // Importe total de la operación.
        public decimal Total { get; set; }

        // Cantidad de productos distintos incluidos en la venta.
        //
        // Ejemplo:
        // Notebook + Mouse = 2 items.
        public int CantidadItems { get; set; }

        // Cantidad total de unidades vendidas.
        //
        // Ejemplo:
        // 2 Notebooks + 3 Mouse = 5 unidades.
        public int CantidadUnidades { get; set; }
    }
}