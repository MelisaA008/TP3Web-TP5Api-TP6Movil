using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ProveedorService
{
    private readonly HttpClient _http;
    public ProveedorService(HttpClient http) => _http = http;

    // Ejemplo de método GET
    public async Task<List<T>> GetAllAsync<T>(string endpoint)
        => await _http.GetFromJsonAsync<List<T>>(endpoint);

    // Ejemplo de método POST
    public async Task AddAsync<T>(string endpoint, T data)
    {
        var response = await _http.PostAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
    }
}
