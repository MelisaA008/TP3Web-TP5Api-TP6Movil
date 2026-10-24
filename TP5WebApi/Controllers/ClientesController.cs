using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP5WebApi.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly DataContext _context;
    public ClientesController(DataContext context) => _context = context;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _context.Clientes.ToListAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _context.Clientes.FindAsync(id));
    [HttpPost] public async Task<IActionResult> Create(Cliente cliente) { _context.Clientes.Add(cliente); await _context.SaveChangesAsync(); return Ok(cliente); }
    [HttpPut("{id}")] public async Task<IActionResult> Update(int id, Cliente cliente) { if (id != cliente.Id) return BadRequest(); _context.Entry(cliente).State = EntityState.Modified; await _context.SaveChangesAsync(); return NoContent(); }
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var cliente = await _context.Clientes.FindAsync(id); if (cliente == null) return NotFound(); _context.Clientes.Remove(cliente); await _context.SaveChangesAsync(); return NoContent(); }
}
