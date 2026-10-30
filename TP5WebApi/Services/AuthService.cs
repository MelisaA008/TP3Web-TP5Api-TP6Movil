using Microsoft.EntityFrameworkCore;
using TP5Shared.DTOs;
using TP5Shared.Models;
using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    public class AuthService
    {
        private readonly DataContext _context;
        private readonly JwtService _jwtService;

        public AuthService(
            DataContext context,
            JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Usuarios
                .AnyAsync(u => u.Username == request.Username))
            {
                return false;
            }

            var usuario = new Usuario
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Rol = "Usuario"
            };

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (usuario == null)
                return null;

            bool passwordValido =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    usuario.PasswordHash);

            if (!passwordValido)
                return null;

            var (token, expiracion) =
                _jwtService.GenerarToken(usuario);

            return new AuthResponseDto
            {
                Token = token,
                Username = usuario.Username ?? "",
                Rol = usuario.Rol,
                Expiracion = expiracion
            };
        }
    }
}