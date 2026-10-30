using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP5Shared.DTOs;
using TP5WebApi.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class SalidasController : ControllerBase
{
    private readonly SalidaService _service;

    public SalidasController(
        SalidaService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarSalida(
        [FromBody] CrearSalidaDto dto)
    {
        var resultado =
            await _service.RegistrarSalida(dto);

        if (resultado == null)
            return BadRequest(
                "No hay stock suficiente o producto inexistente");

        return Ok(resultado);
    }
}