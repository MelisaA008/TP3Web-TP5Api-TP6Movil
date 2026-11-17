using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP5Shared.DTOs;
using TP5WebApi.Services;

namespace TP5WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]

[Authorize]
public class ReportesController : ControllerBase
{
    private readonly ReportePdfService _service;
    private readonly EmailService _emailService;

    // sumar al constructor existente:
    public ReportesController(ReportePdfService service, EmailService emailService)
    {
        _service = service;
        _emailService = emailService;
    }

    [HttpGet("stock/pdf")]
   
    public async Task<IActionResult> Stock()
        => File(await _service.GenerarReporteStock(), "application/pdf", "reporte_stock.pdf");

    [HttpGet("ingresos/pdf")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Ingresos([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        => File(await _service.GenerarReporteIngresos(desde, hasta), "application/pdf", "reporte_ingresos.pdf");

    [HttpGet("ventas/pdf")]
    
    public async Task<IActionResult> Ventas([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    => File(await _service.GenerarReporteVentas(desde, hasta), "application/pdf", "reporte_ventas.pdf");

    [HttpGet("ventas/hoy/pdf")]
    public async Task<IActionResult> VentasHoy()
    {
        var hoy = DateTime.Today;
        var manana = hoy.AddDays(1);
        return File(await _service.GenerarReporteVentas(hoy, manana), "application/pdf", $"reporte_ventas_{hoy:yyyyMMdd}.pdf");
    }

    [HttpGet("salidas/pdf")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Salidas([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    => File(await _service.GenerarReporteSalidas(desde, hasta), "application/pdf", "reporte_salidas.pdf");

    [HttpGet("venta/{id}/ticket")]
    public async Task<IActionResult> Ticket(int id)
    {
        var pdf = await _service.GenerarTicketVenta(id);
        if (pdf == null) return NotFound();
        return File(pdf, "application/pdf", $"ticket_{id}.pdf");
    }

    [HttpPost("{tipo}/email")]
    public async Task<IActionResult> EnviarPorEmail(string tipo, [FromBody] EnviarReporteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Destinatario))
            return BadRequest("Falta el destinatario");

        var tipoLower = tipo.ToLower();

        if ((tipoLower == "ingresos" || tipoLower == "salidas") && !User.IsInRole("Admin"))
            return Forbid();

        byte[] pdf;
        string nombre;
        var hoy = DateTime.Today;

        switch (tipoLower)
        {
            case "ingresos":
                pdf = await _service.GenerarReporteIngresos(null, null);
                nombre = "reporte_ingresos.pdf";
                break;
            case "stock":
                pdf = await _service.GenerarReporteStock();
                nombre = "reporte_stock.pdf";
                break;
            case "ventas":
                pdf = await _service.GenerarReporteVentas(null, null);
                nombre = "reporte_ventas.pdf";
                break;
            case "ventashoy":
                pdf = await _service.GenerarReporteVentas(hoy, hoy.AddDays(1));
                nombre = $"reporte_ventas_{hoy:yyyyMMdd}.pdf";
                break;
            case "salidas":
                pdf = await _service.GenerarReporteSalidas(null, null);
                nombre = "reporte_salidas.pdf";
                break;
            default:
                return BadRequest("Tipo de reporte inválido");
        }

        await _emailService.EnviarPdf(dto.Destinatario, $"Reporte: {tipo}", "Adjunto el reporte solicitado.", pdf, nombre);
        return Ok(new { mensaje = "Reporte enviado correctamente" });
    }
}