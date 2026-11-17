using System.Net.Http.Json;

using TP5Shared.DTOs;

namespace TP5WebAppWasm.Services;

/// <summary>
/// Operaciones relacionadas con ventas.
/// </summary>
public class VentaService
{
    private readonly HttpClient _http;

    public VentaService(
        HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Registra una venta.
    /// </summary>
    public async Task<bool>
        RegistrarVenta(
            CrearVentaDto dto)
    {
        var response =
            await _http.PostAsJsonAsync(
                "api/Ventas",
                dto);

        return response
            .IsSuccessStatusCode;
    }

    /// <summary>
    /// Obtiene ventas registradas.
    /// </summary>
    public async Task<List<VentaListaDto>?>
        GetVentas()
    {
        return await _http
            .GetFromJsonAsync<
                List<VentaListaDto>>
            (
                "api/Ventas"
            );
    }
    /// <summary>
    /// Registra una venta y descarga el ticket.
    /// </summary>
    public async Task<int?> RegistrarVentaConId(CrearVentaDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/Ventas", dto);
        if (!response.IsSuccessStatusCode) return null;

        var resultado = await response.Content.ReadFromJsonAsync<VentaRespuesta>();
        return resultado?.Venta?.Id;
    }

    private class VentaRespuesta
    {
        public string? Mensaje { get; set; }
        public VentaDto? Venta { get; set; }
    }
}