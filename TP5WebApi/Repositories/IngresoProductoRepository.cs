using TP5WebApi.Models;
using TP5WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TP5WebApi.Repositories
{
    public class IngresoProductoRepository
    {
        private readonly DataContext _context;
        public IngresoProductoRepository(DataContext context) => _context = context;

        public async Task<List<IngresoProducto>> GetAllAsync() => await _context.IngresosProductos.ToListAsync();
        public async Task AddAsync(IngresoProducto ingreso)
        {
            _context.IngresosProductos.Add(ingreso);
            await _context.SaveChangesAsync();
        }
    }
}