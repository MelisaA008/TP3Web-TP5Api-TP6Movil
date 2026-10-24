using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP5WebApi.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProveedoresController : ControllerBase
{
    private readonly DataContext _context;
    public ProveedoresController(DataContext context) => _context = context;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _context.Proveedores.ToListAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _context.Proveedores.FindAsync(id));
    [HttpPost] public async Task<IActionResult> Create(Proveedor proveedor) { _context.Proveedores.Add(proveedor); await _context.SaveChangesAsync(); return Ok(proveedor); }
    [HttpPut("{id}")] public async Task<IActionResult> Update(int id, Proveedor proveedor) { if (id != proveedor.Id) return BadRequest(); _context.Entry(proveedor).State = EntityState.Modified; await _context.SaveChangesAsync(); return NoContent(); }
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var proveedor = await _context.Proveedores.FindAsync(id); if (proveedor == null) return NotFound(); _context.Proveedores.Remove(proveedor); await _context.SaveChangesAsync(); return NoContent(); }
}
