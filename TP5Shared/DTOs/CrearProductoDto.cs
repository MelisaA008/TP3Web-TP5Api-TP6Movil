namespace TP5Shared.DTOs
{
    /// <summary>
    /// Datos necesarios para registrar un producto nuevo.
    ///
    /// Se utiliza en las pantallas de alta y al enviar información a la API.
    //  Un producto pertenece a una categoría y está asociado a un proveedor.
    //
    //  El stock luego se modifica mediante:
    //  - ingresos de mercadería
    //  - salidas
    //  - ventas
    /// </summary>
    public class CrearProductoDto
    {
        // Nombre del producto.
        //
        // Ejemplos:
        // Notebook Lenovo
        // Mouse Inalámbrico
        // Impresora Epson
        public string Nombre { get; set; } = string.Empty;

        // Stock inicial del producto.
        //
        // Representa la cantidad disponible
        // al momento de crear el registro.
        public int Stock { get; set; }

        // Precio de venta del producto.
        public decimal Precio { get; set; }

        // Categoría a la que pertenece.
        public int CategoriaId { get; set; }

        // Proveedor principal del producto.
        public int ProveedorId { get; set; }
    }
}