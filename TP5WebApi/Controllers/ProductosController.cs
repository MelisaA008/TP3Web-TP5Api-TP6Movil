using Microsoft.AspNetCore.Mvc;
using TP5.Shared.DTOs;
using TP5WebApi.Services;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ProductoService _service;

    public ProductosController(ProductoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> Get(
        int pageNumber = 1,
        int pageSize = 10)
    {
        var productos =
            await _service.GetAll(
                pageNumber,
                pageSize);

        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDto>> GetById(
        int id)
    {
        var producto =
            await _service.GetById(id);

        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CrearProductoDto dto)
    {
        var nuevo =
            await _service.Create(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = nuevo.Id },
            nuevo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CrearProductoDto dto)
    {
        var actualizado =
            await _service.Update(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id)
    {
        var eliminado =
            await _service.Delete(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}