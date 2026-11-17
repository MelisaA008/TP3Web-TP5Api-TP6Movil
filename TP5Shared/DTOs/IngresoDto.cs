namespace TP5Shared.DTOs
{
    /// <summary>
    /// Representa una entrada de mercadería.
    ///
    /// Cada ingreso aumenta el stock del producto
    /// y queda asociado al proveedor que realizó la entrega.
    /// </summary>
    public class IngresoDto
    {
        // Identificador único del ingreso.
        public int Id { get; set; }

        // Producto que ingresa al stock.
        public int ProductoId { get; set; }

        // Nombre del producto para mostrar en listados y reportes.
        public string ProductoNombre { get; set; } = string.Empty;

        // Proveedor que realizó la entrega.
        public int ProveedorId { get; set; }

        // Nombre del proveedor para consultas y reportes.
        public string ProveedorNombre { get; set; } = string.Empty;

        // Cantidad incorporada al stock.
        public int Cantidad { get; set; }

        // Fecha en la que se registró el ingreso.
        public DateTime Fecha { get; set; }
    }
}