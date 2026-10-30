namespace TP5WebApp.Services;
public class SalidaService
{
    private readonly HttpClient _http;
    public SalidaService(HttpClient http) => _http = http;

    // Ejemplo de método GET
    /// <summary>
    /// Obtiene todos los registros de un endpoint.
    /// Devuelve una lista vacía si no hay datos.
    /// </summary>
    public async Task<List<T>> GetAllAsync<T>(string endpoint)
        => await _http.GetFromJsonAsync<List<T>>(endpoint)
           ?? [];

    // Ejemplo de método POST
    public async Task AddAsync<T>(string endpoint, T data)
    {
        var response = await _http.PostAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
    }
}
