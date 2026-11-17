using System.Net.Http.Json;

using TP5Shared.DTOs;

namespace TP5WebAppWasm.Services;

public class ClienteService
{
    private readonly HttpClient _http;

    public ClienteService(
        HttpClient http)
    {
        _http = http;
    }
    public async Task<List<ClienteDto>?> GetClientes()
    {
        var resp = await _http.GetFromJsonAsync<RespuestaPaginadaDto<ClienteDto>>("api/Clientes?tamanoPagina=50");
        return resp?.Datos.ToList();
    }

    public async Task<ClienteDto?>
        GetCliente(
            int id)
    {
        return await _http
            .GetFromJsonAsync<
                ClienteDto>
            (
                $"api/Clientes/{id}"
            );
    }

    public async Task<bool>
        CrearCliente(
            CrearClienteDto dto)
    {
        var response =
            await _http.PostAsJsonAsync(
                "api/Clientes",
                dto);

        return response
            .IsSuccessStatusCode;
    }

    public async Task<bool>ActualizarCliente(int id, CrearClienteDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/Clientes/{id}", dto);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool>EliminarCliente(int id)
    {
        var response = await _http.DeleteAsync($"api/Clientes/{id}");

        return response.IsSuccessStatusCode;
    }
    
}