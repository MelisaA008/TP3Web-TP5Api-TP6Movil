using TP5.Shared.Models;
using Microsoft.EntityFrameworkCore;

public class VentaService
{
    private readonly DataContext _context;

    public VentaService(DataContext context)
    {
        _context = context;
    }

    public async Task<Venta> RegistrarVentaAsync(Venta venta)
    {
        if (venta.Detalles == null || !venta.Detalles.Any())
            throw new Exception("La venta debe contener al menos un detalle");

        // Validar cliente
        var cliente = await _context.Clientes.FindAsync(venta.ClienteId);
        if (cliente == null)
            throw new Exception("Cliente no encontrado");

        venta.Fecha = DateTime.Now;
        venta.Total = 0;

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach (var detalle in venta.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                if (producto == null)
                    throw new Exception($"Producto {detalle.ProductoId} no encontrado");

                if (producto.Stock < detalle.Cantidad)
                    throw new Exception($"No hay stock suficiente para {producto.Nombre}");

                // Descontar stock
                producto.Stock -= detalle.Cantidad;

                // Calcular subtotal
                detalle.PrecioUnitario = producto.Precio;
                venta.Total += detalle.Cantidad * detalle.PrecioUnitario;

                // Vincular relaciones
                detalle.Producto = producto;
                detalle.Venta = venta;
            }

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return venta;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<List<Venta>> ObtenerVentasAsync()
    {
        return await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
            .ToListAsync();
    }

    public async Task<Venta?> ObtenerVentaPorIdAsync(int id)
    {
        return await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
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
