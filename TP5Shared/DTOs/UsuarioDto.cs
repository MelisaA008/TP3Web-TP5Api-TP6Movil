namespace TP5Shared.DTOs
{
    /// <summary>
    /// Información de un usuario registrado.
    ///
    /// Se utiliza para administración, listados y control de roles.
    /// </summary>
    public class UsuarioDto
    {
        // Identificador único del usuario.
        public int Id { get; set; }

        // Nombre de usuario utilizado para iniciar sesión.
        public string Username { get; set; } = string.Empty;

        // Correo electrónico asociado a la cuenta.
        public string Email { get; set; } = string.Empty;

        // Rol asignado al usuario.
        //
        // Ejemplos:
        // Admin
        // Usuario
        public string Rol { get; set; } = string.Empty;
    }
}