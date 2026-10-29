namespace TP5.Shared.Models
{
    // -------------------------
    // Usuarios
    // -------------------------

    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string? Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required]
        public string? PasswordHash { get; set; } = string.Empty; // Hash seguro de la contraseña

        [Required, MaxLength(20)]
        public string Rol { get; set; } = "Usuario"; // Ejemplo: "Admin", "Usuario"
    }
}