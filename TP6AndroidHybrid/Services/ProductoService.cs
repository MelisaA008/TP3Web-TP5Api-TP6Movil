using System.Net.Http.Headers;
using System.Net.Http.Json;
using TP5Shared.DTOs;
using Microsoft.AspNetCore.Components.Forms;


namespace TP6AndroidHybrid.Services;

/// <summary>
/// Maneja todas las operaciones de productos.
/// </summary>
public class ProductoService
{
    private readonly HttpClient _http;

    public ProductoService(
        HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Obtiene productos paginados.
    /// </summary>
    public async Task<
        RespuestaPaginadaDto<ProductoDto>?>
        GetProductosPaginados(
            ParametrosPaginacionDto p)
    {
        var url =
            $"api/productos?" +
            $"pagina={p.Pagina}" +
            $"&tamanoPagina={p.TamanoPagina}" +
            $"&buscar={p.Buscar}" +
            $"&categoriaId={p.CategoriaId}" +
            $"&ordenarPor={p.OrdenarPor}";

        return await _http
            .GetFromJsonAsync<
                RespuestaPaginadaDto<
                    ProductoDto>>
                    (url);
    }

    /// <summary>
    /// Crear producto.
    /// </summary>
    public async Task<ProductoDto?>
    CrearProducto(
        CrearProductoDto dto)
    {
        var response =
            await _http.PostAsJsonAsync(
                "api/Productos",
                dto);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content
            .ReadFromJsonAsync<
                ProductoDto>();
    }

    /// <summary>
    /// Subir imagen de producto.
    /// </summary>

    /*public async Task<bool>
        SubirImagen(
            int productoId,
            IBrowserFile file)
    {
        using var content =
            new MultipartFormDataContent();

        var fileContent =
            new StreamContent(
                file.OpenReadStream(
                    maxAllowedSize:
                    5 * 1024 * 1024));

        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue(
                file.ContentType);

        content.Add(
            fileContent,
            "archivo",
            file.Name);

        var response =
            await _http.PostAsync(
                $"api/productos/{productoId}/imagenes",
                content);

        return response.IsSuccessStatusCode;
    }*/

    // prueba respuesta real
    public async Task<bool>
    SubirImagen(
        int productoId,
        IBrowserFile file)
    {
        try
        {
            Console.WriteLine(
                $"[CLIENTE] Subiendo imagen a producto {productoId}");

            using var content =
                new MultipartFormDataContent();

            var fileContent =
                new StreamContent(
                    file.OpenReadStream(
                        5 * 1024 * 1024));

            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(
                    file.ContentType);

            content.Add(
                fileContent,
                "archivo",
                file.Name);

            var response =
                await _http.PostAsync(
                    $"api/productos/{productoId}/imagenes",
                    content);

            Console.WriteLine(
                $"[CLIENTE] Status: {(int)response.StatusCode}");

            var texto =
                await response.Content.ReadAsStringAsync();

            Console.WriteLine(
                $"[CLIENTE] Respuesta: {texto}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[CLIENTE ERROR] {ex}");

            return false;
        }
    }

    /// <summary>
    /// Actualiza un producto existente.
    /// </summary>
    public async Task<bool>
        ActualizarProducto(
            int id,
            CrearProductoDto dto)
    {
        var response =
            await _http.PutAsJsonAsync(
                $"api/productos/{id}",
                dto);

        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Elimina un producto.
    /// </summary>
    public async Task<bool>
        EliminarProducto(
            int id)
    {
        var response =
            await _http.DeleteAsync(
                $"api/productos/{id}");

        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Obtiene todos los productos para dashboard
    /// y consultas rápidas.
    /// </summary>
    public async Task<List<ProductoDto>>
        GetProductosAsync()
    {
        var respuesta =
            await GetProductosPaginados(
                new ParametrosPaginacionDto
                {
                    Pagina = 1,
                    TamanoPagina = 50
                });

        return respuesta?
            .Datos
            .ToList()
            ?? new List<ProductoDto>();
    }

}

