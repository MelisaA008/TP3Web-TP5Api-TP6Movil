using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TP5Shared.DTOs;
using TP5Shared.Models;
using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    /// <summary>
    /// Maneja autenticación y consultas de usuarios.
    /// </summary>
    public class AuthService
    {
        private readonly DataContext _context;

        private readonly JwtService _jwtService;

        public AuthService (DataContext context, JwtService jwtService, ILogger<AuthService> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }

        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Registra un usuario nuevo.
        /// </summary>
        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Intento de registro para usuario {Username}", request.Username);

                if (await _context.Usuarios.AnyAsync(u => u.Username == request.Username))
                {
                    _logger.LogWarning("Registro rechazado. Usuario ya existe: {Username}", request.Username);

                    return false;
                }

                var usuario = new Usuario
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password), Rol = "Usuario"
                };

                _context.Usuarios.Add(usuario);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuario registrado correctamente: {Username}", usuario.Username);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando usuario {Username}", request.Username);

                throw;
            }
        }


        /// <summary>
        /// Valida credenciales
        /// y devuelve JWT.
        /// </summary>
        public async Task<AuthResponseDto?> LoginAsync(
     LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Intento de login para usuario {Username}", request.Username);

                var usuario = await _context.Usuarios
                        .FirstOrDefaultAsync( u => u.Username == request.Username);

                if (usuario == null)
                {
                    _logger.LogWarning("Login fallido. Usuario inexistente: {Username}", request.Username);

                    return null;
                }

                bool passwordValido = BCrypt.Net.BCrypt.Verify( request.Password, usuario.PasswordHash);

                if (!passwordValido)
                {
                    _logger.LogWarning(
                        "Login fallido. Contraseña inválida para {Username}", request.Username);

                    return null;
                }

                var (token, expiracion) = _jwtService.GenerarToken(usuario);

                _logger.LogInformation(
                    "Login exitoso para {Username} con rol {Rol}", usuario.Username, usuario.Rol);

                return new AuthResponseDto
                {
                    Token = token,
                    Username = usuario.Username ?? "",
                    Rol = usuario.Rol,
                    Expiracion = expiracion
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante login de {Username}", request.Username);
                
                throw;
            }
        }


        /// <summary>
        /// Obtiene todos los usuarios.
        /// </summary>
        public async Task<List<UsuarioDto>>
    GetUsuariosAsync()
        {
            try
            {
                return await _context.Usuarios
                    .Select(u => new UsuarioDto
                        {
                            Id = u.Id,
                            Username = u.Username ?? "",
                            Email = u.Email ?? "",
                            Rol = u.Rol ?? ""
                        })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error obteniendo usuarios");

                return new List<UsuarioDto>();
            }
        }

        /// <summary>
        /// Obtiene un usuario por Id.
        /// </summary>
        public async Task<UsuarioDto?>
    GetUsuarioAsync(int id)
        {
            try
            {
                return await _context.Usuarios
                    .Where(u => u.Id == id)
                    .Select(u => new UsuarioDto
                        {
                            Id = u.Id,

                            Username = u.Username ?? string.Empty,

                            Email = u.Email ?? string.Empty,

                            Rol = u.Rol ?? "Usuario"
                        })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error obteniendo usuario {Id}", id);

                return null;
            }
        }
    }
}