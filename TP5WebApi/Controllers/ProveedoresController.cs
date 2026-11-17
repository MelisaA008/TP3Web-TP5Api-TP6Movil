using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP5Shared.DTOs;
using TP5WebApi.Services;

namespace TP5WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProveedoresController : ControllerBase
{
    private readonly ProveedorService _service;
    public ProveedoresController(ProveedorService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<RespuestaPaginadaDto<ProveedorDto>>> Get([FromQuery] ParametrosPaginacionDto parametros)
        => Ok(await _service.GetAllPaginado(parametros));

    [HttpGet("{id}")]
    public async Task<ActionResult<ProveedorDto>> GetById(int id)
    {
        var proveedor = await _service.GetById(id);
        return proveedor == null ? NotFound() : Ok(proveedor);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearProveedorDto dto)
    {
        var nuevo = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = nuevo.Id }, nuevo);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CrearProveedorDto dto)
        => await _service.Update(id, dto) ? NoContent() : NotFound();

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.Delete(id) ? NoContent() : NotFound();
}
