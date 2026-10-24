using Microsoft.AspNetCore.Mvc;
using TP5WebApi.Models;
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
    public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10)
    {
        var productos = await _service.GetAll(pageNumber, pageSize);
        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _service.GetById(id);
        if (producto == null) return NotFound();
        return Ok(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Producto producto)
    {
        var nuevo = await _service.Create(producto);
        return CreatedAtAction(nameof(GetById), new { id = nuevo.Id }, nuevo);
    }
}
