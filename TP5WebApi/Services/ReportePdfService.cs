using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TP5WebApi.Data;

namespace TP5WebApi.Services;

public class ReportePdfService
{
    private readonly DataContext _context;
    public ReportePdfService(DataContext context) => _context = context;

    public async Task<byte[]> GenerarReporteStock()
    {
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .Include(p => p.Proveedor)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return ConstruirPdf("Reporte de Stock", "Producto|Categoría|Proveedor|Stock|Precio",
            productos.Select(p => new[]
            {
                p.Nombre, p.Categoria?.Nombre ?? "-", p.Proveedor?.Nombre ?? "-",
                p.Stock.ToString(), $"$ {p.Precio:N2}"
            }));
    }

    public async Task<byte[]> GenerarReporteIngresos(DateTime? desde, DateTime? hasta)
    {
        var query = _context.IngresosProductos
            .Include(i => i.Producto)
            .Include(i => i.Proveedor)
            .AsQueryable();

        if (desde.HasValue) query = query.Where(i => i.Fecha >= desde.Value);
        if (hasta.HasValue) query = query.Where(i => i.Fecha <= hasta.Value);

        var ingresos = await query.OrderByDescending(i => i.Fecha).ToListAsync();

        return ConstruirPdf("Reporte de Ingresos", "Fecha|Producto|Proveedor|Cantidad",
            ingresos.Select(i => new[]
            {
                i.Fecha.ToString("dd/MM/yyyy HH:mm"), i.Producto?.Nombre ?? "-",
                i.Proveedor?.Nombre ?? "-", i.Cantidad.ToString()
            }));
    }

    public async Task<byte[]> GenerarReporteVentas(DateTime? desde, DateTime? hasta)
    {
        var query = _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .AsQueryable();

        if (desde.HasValue) query = query.Where(v => v.Fecha >= desde.Value);
        if (hasta.HasValue) query = query.Where(v => v.Fecha <= hasta.Value);

        var ventas = await query.OrderByDescending(v => v.Fecha).ToListAsync();

        var filas = ventas.SelectMany(v => v.Detalles.Select(d => new[]
        {
            v.Fecha.ToString("dd/MM/yyyy HH:mm"),
            v.Cliente?.Nombre ?? "-",
            d.Producto?.Nombre ?? "-",
            d.Cantidad.ToString(),
            $"$ {d.PrecioUnitario * d.Cantidad:N2}"
        }));

        return ConstruirPdf("Reporte de Ventas / Salidas", "Fecha|Cliente|Producto|Cantidad|Subtotal", filas);
    }

    private byte[] ConstruirPdf(string titulo, string encabezados, IEnumerable<string[]> filas)
    {
        var columnas = encabezados.Split('|');

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(col =>
                {
                    col.Item().Text(titulo).FontSize(18).Bold();
                    col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(9);
                });

                page.Content().PaddingTop(15).Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        foreach (var _ in columnas) c.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        foreach (var titulo2 in columnas)
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text(titulo2).Bold();
                    });

                    foreach (var fila in filas)
                        foreach (var valor in fila)
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(valor);
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                    x.Span(" de ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    public async Task<byte[]> GenerarReporteSalidas(DateTime? desde, DateTime? hasta)
    {
        var query = _context.SalidasProductos
            .Include(s => s.Producto)
            .Include(s => s.Cliente)
            .AsQueryable();

        if (desde.HasValue) query = query.Where(s => s.Fecha >= desde.Value);
        if (hasta.HasValue) query = query.Where(s => s.Fecha <= hasta.Value);

        var salidas = await query.OrderByDescending(s => s.Fecha).ToListAsync();

        return ConstruirPdf("Reporte de Salidas de Stock", "Fecha|Producto|Cliente|Cantidad",
            salidas.Select(s => new[]
            {
            s.Fecha.ToString("dd/MM/yyyy HH:mm"),
            s.Producto?.Nombre ?? "-",
            s.Cliente?.Nombre ?? "-",
            s.Cantidad.ToString()
            }));
    }
    public async Task<byte[]?> GenerarTicketVenta(int ventaId)
    {
        var venta = await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles).ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.Id == ventaId);

        if (venta == null) return null;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A6);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Content().Column(col =>
                {
                    col.Item().Text("Sistema de Gestión Comercial").Bold().FontSize(13);
                    col.Item().Text($"Ticket N° {venta.Id}").FontSize(9);
                    col.Item().Text($"Fecha: {venta.Fecha:dd/MM/yyyy HH:mm}").FontSize(9);
                    col.Item().Text($"Cliente: {venta.Cliente?.Nombre ?? "-"}").FontSize(9);
                    col.Item().PaddingVertical(5).LineHorizontal(1);

                    foreach (var d in venta.Detalles)
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"{d.Producto?.Nombre} x{d.Cantidad}");
                            row.ConstantItem(70).AlignRight().Text($"$ {d.PrecioUnitario * d.Cantidad:N2}");
                        });
                    }

                    col.Item().PaddingVertical(5).LineHorizontal(1);
                    col.Item().AlignRight().Text($"TOTAL: $ {venta.Total:N2}").Bold().FontSize(12);
                    col.Item().PaddingTop(10).AlignCenter().Text("¡Gracias por su compra!").FontSize(9);
                });
            });
        }).GeneratePdf();
    }
}