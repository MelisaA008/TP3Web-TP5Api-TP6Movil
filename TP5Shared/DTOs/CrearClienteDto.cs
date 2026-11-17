namespace TP5Shared.DTOs
{
    /// <summary>
    /// Datos necesarios para registrar un cliente nuevo.
    /// </summary>
    public class CrearClienteDto
    {
        // Nombre o razón social.
        public string Nombre { get; set; } = string.Empty;

        // Información de contacto.
        public string Contacto { get; set; } = string.Empty;
    }
}