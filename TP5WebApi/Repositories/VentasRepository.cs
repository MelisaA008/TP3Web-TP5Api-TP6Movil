using TP5WebApi.Repositories;

public class VentasRepository : IVentasRepository
{
    private readonly DataContext _context;

    public VentasRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Venta> RegistrarVentaAsync(Venta venta)
    {
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();
        return venta;
    }

    public async Task<List<Venta>> ObtenerVentasAsync()
    {
        return await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Venta?> ObtenerVentaPorIdAsync(int id)
    {
        return await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<bool> EliminarVentaAsync(int id)
    {
        var venta = await _context.Ventas.FindAsync(id);
        if (venta == null) return false;

        _context.Ventas.Remove(venta);
        await _context.SaveChangesAsync();
        return true;
    }
}