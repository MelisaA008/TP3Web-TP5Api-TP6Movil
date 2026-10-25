using Microsoft.EntityFrameworkCore;
using TP5.Shared.Models;
using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    public class ClienteService
    {
        private readonly DataContext _context;
        public ClienteService(DataContext context) => _context = context;

        public async Task<List<Cliente>> GetAll() => await _context.Clientes.ToListAsync();
        public async Task<Cliente?> GetById(int id) => await _context.Clientes.FindAsync(id);
        public async Task<Cliente> Create(Cliente cliente) { _context.Clientes.Add(cliente); await _context.SaveChangesAsync(); return cliente; }
        public async Task<bool> Update(Cliente cliente) { _context.Entry(cliente).State = EntityState.Modified; await _context.SaveChangesAsync(); return true; }
        public async Task<bool> Delete(int id) { var cliente = await _context.Clientes.FindAsync(id); if (cliente == null) return false; _context.Clientes.Remove(cliente); await _context.SaveChangesAsync(); return true; }
    }
}
