namespace TP5Shared.DTOs
{
    /// <summary>
    /// Información de un proveedor registrado.
    ///
    /// Se utiliza para listados,
    /// consultas y selección de proveedores.
    /// </summary>
    public class ProveedorDto
    {
        // Identificador único del proveedor.
        public int Id { get; set; }

        // Nombre de la empresa o proveedor.
        public string Nombre { get; set; } = string.Empty;

        // Información de contacto.
      
        public string Contacto { get; set; } = string.Empty;
    }
}