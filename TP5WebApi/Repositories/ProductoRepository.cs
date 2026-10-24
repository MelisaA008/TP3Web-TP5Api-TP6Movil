using TP5WebApi.Models;
using TP5WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TP5WebApi.Repositories
{
    public class ProductoRepository
    {
        private readonly DataContext _context;

        public ProductoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> GetAllAsync() => await _context.Productos.ToListAsync();
        public async Task<Producto?> GetByIdAsync(int id) => await _context.Productos.FindAsync(id);

        public async Task AddAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}