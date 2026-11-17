using System.Net.Http.Json;

using TP5Shared.DTOs;

namespace TP6AndroidHybrid.Services;

public class ProveedorService
{
    private readonly HttpClient _http;

    public ProveedorService(
        HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ProveedorDto>?>
     GetProveedores()
    {
        var resp = await _http
            .GetFromJsonAsync<RespuestaPaginadaDto<ProveedorDto>>(
                "api/Proveedores?tamanoPagina=50");

        return resp?.Datos.ToList();
    }

    public async Task<ProveedorDto?>
        GetProveedor(
            int id)
    {
        return await _http
            .GetFromJsonAsync<
                ProveedorDto>
            (
                $"api/Proveedores/{id}"
            );
    }

    public async Task<bool>
        CrearProveedor(
            CrearProveedorDto dto)
    {
        var response =
            await _http.PostAsJsonAsync(
                "api/Proveedores",
                dto);

        return response
            .IsSuccessStatusCode;
    }

    public async Task<bool>
        ActualizarProveedor(
            int id,
            CrearProveedorDto dto)
    {
        var response =
            await _http.PutAsJsonAsync(
                $"api/Proveedores/{id}",
                dto);

        return response
            .IsSuccessStatusCode;
    }

    public async Task<bool>
        EliminarProveedor(
            int id)
    {
        var response =
            await _http.DeleteAsync(
                $"api/Proveedores/{id}");

        return response
            .IsSuccessStatusCode;
    }
}