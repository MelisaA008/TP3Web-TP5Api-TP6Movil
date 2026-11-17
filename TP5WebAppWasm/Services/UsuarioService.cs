using System.Net.Http.Json;
using TP5Shared.DTOs;

namespace TP5WebAppWasm.Services;

/// <summary>
/// Operaciones relacionadas con usuarios.
/// </summary>
public class UsuarioService
{
    private readonly HttpClient _http;

    public UsuarioService(
        HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Obtiene todos los usuarios.
    /// </summary>
    public async Task<List<UsuarioDto>?>
        GetUsuarios()
    {
        return await _http
            .GetFromJsonAsync<
                List<UsuarioDto>>
            (
                "api/Usuarios"
            );
    }

    /// <summary>
    /// Obtiene un usuario por Id.
    /// </summary>
    public async Task<UsuarioDto?>
        GetUsuario(
            int id)
    {
        return await _http
            .GetFromJsonAsync<
                UsuarioDto>
            (
                $"api/Usuarios/{id}"
            );
    }
}