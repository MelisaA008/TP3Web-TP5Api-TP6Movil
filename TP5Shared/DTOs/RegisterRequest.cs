namespace TP5Shared.DTOs
{
    /// <summary>
    /// Datos necesarios para registrar un usuario nuevo.
    /// </summary>
    public class RegisterRequest
    {
        // Nombre de usuario para acceder al sistema.
        public string Username { get; set; } = string.Empty;

        // Correo electrónico del usuario.
        public string Email { get; set; } = string.Empty;

        // Contraseña elegida para la cuenta.
        public string Password { get; set; } = string.Empty;
    }
}