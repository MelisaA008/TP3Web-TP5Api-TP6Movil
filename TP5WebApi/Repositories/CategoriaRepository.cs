using TP5WebApi.Models;
using TP5WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TP5WebApi.Repositories
{
    public class CategoriaRepository
    {
        private readonly DataContext _context;
        public CategoriaRepository(DataContext context) => _context = context;

        public async Task<List<Categoria>> GetAllAsync() => await _context.Categorias.ToListAsync();
        public async Task<Categoria?> GetByIdAsync(int id) => await _context.Categorias.FindAsync(id);
    }
}