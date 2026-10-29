using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TP5WebApi.Services
{
    public class UsuarioService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Registro de usuario
        public async Task<string> Register(Usuario usuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Username == usuario.Username))
                return "El usuario ya existe";

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return "Usuario registrado correctamente";
        }

        // Login y generación de token
        public async Task<string?> Login(string username, string password)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
            if (usuario == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
                return null;

            return GenerateJwtToken(usuario);
        }

        // Generación del JWT
        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Username ?? string.Empty),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
