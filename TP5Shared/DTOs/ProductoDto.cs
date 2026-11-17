namespace TP5Shared.DTOs
{
    /// <summary>
    /// Información completa de un producto.
    ///
    /// Se utiliza para mostrar datos en listados, consultas y reportes.
    /// </summary>
    public class ProductoDto
    {
        // Identificador único del producto.
        public int Id { get; set; }

        // Nombre del producto.
        public string Nombre { get; set; } = string.Empty;

        // Stock disponible actualmente.
        //
        // Se actualiza mediante:
        // - ingresos
        // - salidas
        // - ventas

        //Imagen asociada al producto, si existe.
        public string? ImagenPath { get; set; }
       
        public int Stock { get; set; }

        // Precio de venta actual.
        public decimal Precio { get; set; }

        // Categoría asociada al producto.
        public int CategoriaId { get; set; }

        // Nombre de la categoría.
        public string CategoriaNombre { get; set; } = string.Empty;

        // Proveedor asociado al producto.
        public int ProveedorId { get; set; }

        // Nombre del proveedor.
        public string ProveedorNombre { get; set; } = string.Empty;
    }
}