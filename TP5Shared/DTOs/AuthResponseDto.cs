namespace TP5Shared.DTOs
{
    /// <summary>
    /// Información devuelta por la API después de una autenticación exitosa.
    /// </summary>
    public class AuthResponseDto
    {
        // Token JWT utilizado para acceder a las rutas protegidas.
        public string Token { get; set; } = string.Empty;

        // Nombre del usuario autenticado.
        public string Username { get; set; } = string.Empty;

        // Rol asignado al usuario.
        //
        // Ejemplos:
        // Admin
        // Usuario
        public string Rol { get; set; } = string.Empty;

        // Fecha y hora de vencimiento del token.
        public DateTime Expiracion { get; set; }
    }
}