using System.Net.Http.Json;

using TP5Shared.DTOs;

namespace TP6AndroidHybrid.Services;

/// <summary>
/// Maneja todas las operaciones de categorías.
/// </summary>
public class CategoriaService
{
    private readonly HttpClient _http;

    public CategoriaService(
        HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Obtiene todas las categorías.
    /// </summary>
    public async Task<List<CategoriaDto>?> GetCategorias()
    {
        var resp = await _http.GetFromJsonAsync<RespuestaPaginadaDto<CategoriaDto>>("api/Categorias?tamanoPagina=50");
        return resp?.Datos.ToList();
    }

    /// <summary>
    /// Obtiene una categoría por Id.
    /// </summary>
    public async Task<CategoriaDto?>
        GetCategoria(
            int id)
    {
        return await _http
            .GetFromJsonAsync<
                CategoriaDto>
            (
                $"api/Categorias/{id}"
            );
    }

    /// <summary>
    /// Crea una categoría nueva.
    /// </summary>
    public async Task<bool>
        CrearCategoria(
            CrearCategoriaDto dto)
    {
        var response =
            await _http.PostAsJsonAsync(
                "api/Categorias",
                dto);

        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Actualiza una categoría existente.
    /// </summary>
    public async Task<bool>
        ActualizarCategoria(
            int id,
            CrearCategoriaDto dto)
    {
        var response =
            await _http.PutAsJsonAsync(
                $"api/Categorias/{id}",
                dto);

        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Elimina una categoría.
    /// </summary>
    public async Task<bool>
        EliminarCategoria(
            int id)
    {
        var response =
            await _http.DeleteAsync(
                $"api/Categorias/{id}");

        return response.IsSuccessStatusCode;
    }
}