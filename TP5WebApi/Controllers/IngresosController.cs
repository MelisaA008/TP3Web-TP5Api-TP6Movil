using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP5Shared.DTOs;
using TP5WebApi.Services;

namespace TP5WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class IngresosController : ControllerBase
{
    private readonly IngresoService _service;
    public IngresosController(IngresoService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAll());

    [HttpPost]
    public async Task<IActionResult> RegistrarIngreso([FromBody] CrearIngresoDto dto)
    {
        var resultado = await _service.RegistrarIngreso(dto);
        return resultado == null
            ? BadRequest("Producto/proveedor inexistente o cantidad inválida")
            : Ok(resultado);
    }
}