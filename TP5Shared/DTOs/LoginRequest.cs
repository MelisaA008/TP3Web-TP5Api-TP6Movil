namespace TP5Shared.DTOs
{
    /// <summary>
    /// Credenciales utilizadas para iniciar sesión.
    /// </summary>
    public class LoginRequest
    {
        // Nombre de usuario registrado.
        public string Username { get; set; } = string.Empty;

        // Contraseña ingresada por el usuario.
        public string Password { get; set; } = string.Empty;
    }
}