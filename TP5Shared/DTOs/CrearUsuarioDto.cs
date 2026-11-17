namespace TP5Shared.DTOs
{
    /// <summary>
    /// Datos necesarios para crear un usuario.
    /// </summary>
    public class CrearUsuarioDto
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Rol { get; set; } = "Usuario";
    }
}