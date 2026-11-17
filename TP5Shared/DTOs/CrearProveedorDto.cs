namespace TP5Shared.DTOs
{
    /// <summary>
    /// Datos necesarios para registrar un proveedor nuevo.
    ///
    /// Se utiliza en los formularios de alta y para enviar información a la API.
    /// </summary>
    public class CrearProveedorDto
    {
        // Nombre del proveedor o empresa
        public string Nombre { get; set; } = string.Empty;

        // Información para comunicarse con el proveedor.
        public string Contacto { get; set; } = string.Empty;
    }
}