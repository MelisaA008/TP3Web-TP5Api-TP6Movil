namespace TP5WebApi.Repositories
{
    public class ClienteRepository
    {
        private readonly DataContext _context;
        public ClienteRepository(DataContext context) => _context = context;

        public async Task<List<Cliente>> GetAllAsync() => await _context.Clientes.ToListAsync();
        public async Task<Cliente?> GetByIdAsync(int id) => await _context.Clientes.FindAsync(id);
    }
}