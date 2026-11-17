using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace TP5WebAppWasm.Services;

public class ReporteService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public ReporteService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    private async Task DescargarPdf(string endpoint, string nombreArchivo)
    {
        var bytes = await _http.GetByteArrayAsync(endpoint);
        using var stream = new MemoryStream(bytes);
        using var streamRef = new DotNetStreamReference(stream);
        await _js.InvokeVoidAsync("downloadFileFromStream", nombreArchivo, streamRef);
    }

    public Task DescargarStock() => DescargarPdf("api/Reportes/stock/pdf", "reporte_stock.pdf");
    public Task DescargarVentas() => DescargarPdf("api/Reportes/ventas/pdf", "reporte_ventas.pdf");
    public Task DescargarVentasHoy() => DescargarPdf("api/Reportes/ventas/hoy/pdf", $"reporte_ventas_{DateTime.Now:yyyyMMdd}.pdf");
    public Task DescargarSalidas() => DescargarPdf("api/Reportes/salidas/pdf", "reporte_salidas.pdf");

    public async Task<bool> EnviarPorCorreo(string tipo, string destinatario)
    {
        var resp = await _http.PostAsJsonAsync($"api/Reportes/{tipo}/email", new { destinatario });
        return resp.IsSuccessStatusCode;
    }

    public Task DescargarTicket(int ventaId) =>
    DescargarPdf($"api/Reportes/venta/{ventaId}/ticket", $"ticket_{ventaId}.pdf");
}