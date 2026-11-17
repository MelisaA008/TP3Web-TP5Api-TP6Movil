namespace TP5Shared.DTOs
{
    /// <summary>
    /// Datos necesarios para registrar un ingreso de mercadería.
    ///
    /// Cada ingreso aumenta el stock del producto
    /// y queda asociado al proveedor que realizó la entrega.
    /// 
    /// // Este DTO registra una entrada de stock.
    //
    // Ingreso  -> suma stock
    // Salida   -> resta stock
    // Venta    -> genera una o varias salidas
    /// </summary>
    public class CrearIngresoDto
    {
        // Producto que ingresa al inventario.
        public int ProductoId { get; set; }

        // Proveedor que entrega la mercadería.
        public int ProveedorId { get; set; }

        // Cantidad de unidades que se agregan al stock.
        //
        // Ejemplo:
        // Stock actual = 10
        // Cantidad = 5
        //
        // Nuevo stock = 15
        public int Cantidad { get; set; }
    }
}
