using TP5WebApi.Models;
using TP5WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TP5WebApi.Repositories
{
    public class SalidaProductoRepository
    {
        private readonly DataContext _context;
        public SalidaProductoRepository(DataContext context) => _context = context;

        public async Task<List<SalidaProducto>> GetAllAsync() => await _context.SalidasProductos.ToListAsync();
        public async Task AddAsync(SalidaProducto salida)
        {
            _context.SalidasProductos.Add(salida);
            await _context.SaveChangesAsync();
        }
    }
}