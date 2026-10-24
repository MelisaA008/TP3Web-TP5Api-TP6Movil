using Microsoft.EntityFrameworkCore;
using TP5WebApi.Models;

namespace TP5WebApi.Services
{
    public class ProveedorService
    {
        private readonly DataContext _context;
        public ProveedorService(DataContext context) => _context = context;

        public async Task<List<Proveedor>> GetAll() => await _context.Proveedores.ToListAsync();
        public async Task<Proveedor?> GetById(int id) => await _context.Proveedores.FindAsync(id);
        public async Task<Proveedor> Create(Proveedor proveedor) { _context.Proveedores.Add(proveedor); await _context.SaveChangesAsync(); return proveedor; }
        public async Task<bool> Update(Proveedor proveedor) { _context.Entry(proveedor).State = EntityState.Modified; await _context.SaveChangesAsync(); return true; }
        public async Task<bool> Delete(int id) { var proveedor = await _context.Proveedores.FindAsync(id); if (proveedor == null) return false; _context.Proveedores.Remove(proveedor); await _context.SaveChangesAsync(); return true; }
    }
}
