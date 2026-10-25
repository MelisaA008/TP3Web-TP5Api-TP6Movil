using Microsoft.EntityFrameworkCore;
using TP5.Shared.Models;
using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    public class CategoriaService
    {
        private readonly DataContext _context;
        public CategoriaService(DataContext context) => _context = context;

        public async Task<List<Categoria>> GetAll() => await _context.Categorias.ToListAsync();
        public async Task<Categoria?> GetById(int id) => await _context.Categorias.FindAsync(id);
        public async Task<Categoria> Create(Categoria categoria) { _context.Categorias.Add(categoria); await _context.SaveChangesAsync(); return categoria; }
        public async Task<bool> Update(Categoria categoria) { _context.Entry(categoria).State = EntityState.Modified; await _context.SaveChangesAsync(); return true; }
        public async Task<bool> Delete(int id) { var categoria = await _context.Categorias.FindAsync(id); if (categoria == null) return false; _context.Categorias.Remove(categoria); await _context.SaveChangesAsync(); return true; }
    }
}
