using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP5WebApi.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalidasController : ControllerBase
{
    private readonly DataContext _context;
    public SalidasController(DataContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> RegistrarSalida(SalidaProducto salida)
    {
        var producto = await _context.Productos.FindAsync(salida.ProductoId);
        if (producto == null) return NotFound("Producto no encontrado");

        if (producto.Stock < salida.Cantidad)
            return BadRequest("No hay stock suficiente");

        producto.Stock -= salida.Cantidad;
        salida.Fecha = DateTime.Now;

        _context.SalidasProductos.Add(salida);
        await _context.SaveChangesAsync();

        return Ok(salida);
    }
}
