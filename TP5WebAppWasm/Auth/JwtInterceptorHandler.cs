using System.Net.Http.Headers;

using Blazored.LocalStorage;

namespace TP5WebAppWasm.Auth;

/// <summary>
/// Agrega automáticamente el JWT a las peticiones HTTP.
/// </summary>
public class JwtInterceptorHandler
    : DelegatingHandler
{
    // Acceso al token guardado en el navegador.
    private readonly ILocalStorageService _localStorage;

    public JwtInterceptorHandler(
        ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    /// <summary>
    /// Se ejecuta antes de enviar cada petición.
    /// </summary>
    protected override async Task<HttpResponseMessage>
        SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
    {
        // Obtener token guardado.
        var token =
            await _localStorage
                .GetItemAsync<string>(
                    "authToken");

        // Agregar JWT al encabezado Authorization.
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }

        // Continuar con la petición.
        return await base.SendAsync(
            request,
            cancellationToken);
    }
}