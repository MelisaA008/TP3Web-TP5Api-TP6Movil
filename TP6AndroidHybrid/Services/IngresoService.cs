using System.Net.Http.Json;

using TP5Shared.DTOs;

namespace TP6AndroidHybrid.Services;

public class IngresoService
{
    private readonly HttpClient _http;

    public IngresoService(
        HttpClient http)
    {
        _http = http;
    }

    public async Task<List<IngresoDto>?>
        GetIngresos()
    {
        return await _http
            .GetFromJsonAsync<
                List<IngresoDto>>
            (
                "api/Ingresos"
            );
    }

    public async Task<bool>
        RegistrarIngreso(
            CrearIngresoDto dto)
    {
        var response =
            await _http.PostAsJsonAsync(
                "api/Ingresos",
                dto);

        return response
            .IsSuccessStatusCode;
    }
}