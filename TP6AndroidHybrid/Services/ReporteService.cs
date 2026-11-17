using System.Net.Http.Json;

namespace TP6AndroidHybrid.Services;

public class ReporteService
{
    private readonly HttpClient _http;

    public ReporteService(HttpClient http) => _http = http;

    private async Task DescargarPdf(string endpoint, string nombre)
    {
        var bytes = await _http.GetByteArrayAsync(endpoint);
        var ruta = Path.Combine(FileSystem.CacheDirectory, nombre);
        await File.WriteAllBytesAsync(ruta, bytes);

        await Launcher.Default.OpenAsync(
            new OpenFileRequest(nombre, new ReadOnlyFile(ruta, "application/pdf")));
    }

    public Task DescargarStock() => DescargarPdf("api/Reportes/stock/pdf", "reporte_stock.pdf");
    public Task DescargarIngresos() => DescargarPdf("api/Reportes/ingresos/pdf", "reporte_ingresos.pdf");
    public Task DescargarVentas() => DescargarPdf("api/Reportes/ventas/pdf", "reporte_ventas.pdf");
    public Task DescargarVentasHoy() => DescargarPdf("api/Reportes/ventas/hoy/pdf", "reporte_ventas_hoy.pdf");
    public Task DescargarSalidas() => DescargarPdf("api/Reportes/salidas/pdf", "reporte_salidas.pdf");
    public Task DescargarTicket(int ventaId) =>
        DescargarPdf($"api/Reportes/venta/{ventaId}/ticket", $"ticket_{ventaId}.pdf");

    public async Task<bool> EnviarPorCorreo(string tipo, string destinatario)
    {
        var resp = await _http.PostAsJsonAsync($"api/Reportes/{tipo}/email", new { destinatario });
        return resp.IsSuccessStatusCode;
    }
}