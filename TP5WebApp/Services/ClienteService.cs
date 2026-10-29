public class ClienteService
{
    private readonly HttpClient _http;
    public ClienteService(HttpClient http) => _http = http;

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
