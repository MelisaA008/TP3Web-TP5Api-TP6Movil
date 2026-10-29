namespace TP5WebApi.Repositories
{
    public class ProveedorRepository
    {
        private readonly DataContext _context;
        public ProveedorRepository(DataContext context) => _context = context;

        public async Task<List<Proveedor>> GetAllAsync() => await _context.Proveedores.ToListAsync();
        public async Task<Proveedor?> GetByIdAsync(int id) => await _context.Proveedores.FindAsync(id);
    }
}