using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class IngresosController : ControllerBase
{
    private readonly DataContext _context;
    public IngresosController(DataContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> RegistrarIngreso(IngresoProducto ingreso)
    {
        var producto = await _context.Productos.FindAsync(ingreso.ProductoId);
        if (producto == null) return NotFound("Producto no encontrado");

        producto.Stock += ingreso.Cantidad;
        ingreso.Fecha = DateTime.Now;

        _context.IngresosProductos.Add(ingreso);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            ingreso.Id,
            ingreso.ProductoId,
            ingreso.ProveedorId,
            ingreso.Cantidad,
            ingreso.Fecha
        });
    }
}
