namespace TP5Shared.DTOs
{
    /// <summary>
    /// Representa una salida de stock.
    ///
    /// Se usa para registrar qué producto salió,
    /// para qué cliente fue y cuándo ocurrió.
    /// </summary>
    public class SalidaDto
    {
        // Identificador único de la salida.
        public int Id { get; set; }

        // Producto afectado por la salida.
        public int ProductoId { get; set; }

        // Nombre del producto para mostrar en pantalla.
        public string ProductoNombre { get; set; } = string.Empty;

        // Cliente asociado a la salida.
        public int ClienteId { get; set; }

        // Nombre del cliente para consultas y reportes.
        public string ClienteNombre { get; set; } = string.Empty;

        // Cantidad descontada del stock.
        public int Cantidad { get; set; }

        // Fecha en la que se realizó la salida.
        public DateTime Fecha { get; set; }
    }
}