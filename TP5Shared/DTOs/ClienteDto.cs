namespace TP5Shared.DTOs
{
    /// <summary>
    /// Información de un cliente registrada en el sistema.
    /// </summary>
    public class ClienteDto
    {
        // Identificador único del cliente.
        public int Id { get; set; }

        // Nombre o razón social.
        public string Nombre { get; set; } = string.Empty;

        // Teléfono, email u otro dato de contacto.
        public string Contacto { get; set; } = string.Empty;
    }
}