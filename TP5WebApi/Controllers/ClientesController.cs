using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP5Shared.DTOs;
using TP5WebApi.Services;

namespace TP5WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _service;
    public ClientesController(ClienteService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<RespuestaPaginadaDto<ClienteDto>>> Get([FromQuery] ParametrosPaginacionDto parametros)
        => Ok(await _service.GetAllPaginado(parametros));

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        var cliente = await _service.GetById(id);
        return cliente == null ? NotFound() : Ok(cliente);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearClienteDto dto)
    {
        var nuevo = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = nuevo.Id }, nuevo);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CrearClienteDto dto)
        => await _service.Update(id, dto) ? NoContent() : NotFound();

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.Delete(id) ? NoContent() : NotFound();
}