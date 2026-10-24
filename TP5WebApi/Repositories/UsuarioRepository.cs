using TP5WebApi.Models;
using TP5WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TP5WebApi.Repositories
{
    public class UsuarioRepository
    {
        private readonly DataContext _context;
        public UsuarioRepository(DataContext context) => _context = context;

        public async Task<Usuario?> GetByUsernameAsync(string username)
            => await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);

        public async Task AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }
    }
}