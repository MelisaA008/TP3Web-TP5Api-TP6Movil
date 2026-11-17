using System.Net.Http.Json;

using TP5Shared.DTOs;

namespace TP5WebAppWasm.Services;

public class SalidaService
{
    private readonly HttpClient _http;

    public SalidaService(
        HttpClient http)
    {
        _http = http;
    }

    public async Task<List<SalidaDto>?>
        GetSalidas()
    {
        return await _http
            .GetFromJsonAsync<
                List<SalidaDto>>
            (
                "api/Salidas"
            );
    }

    public async Task<bool>
        RegistrarSalida(
            CrearSalidaDto dto)
    {
        var response =
            await _http.PostAsJsonAsync(
                "api/Salidas",
                dto);

        return response
            .IsSuccessStatusCode;
    }
}