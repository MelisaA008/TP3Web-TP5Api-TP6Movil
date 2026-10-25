using TP5.Shared.Models;
using Microsoft.EntityFrameworkCore;
using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    public class IngresoService
    {
        private readonly DataContext _context;
        public IngresoService(DataContext context) => _context = context;

        public async Task<IngresoProducto?> RegistrarIngreso(IngresoProducto ingreso)
        {
            var producto = await _context.Productos.FindAsync(ingreso.ProductoId);
            if (producto == null) return null;

            producto.Stock += ingreso.Cantidad;
            ingreso.Fecha = DateTime.Now;

            _context.IngresosProductos.Add(ingreso);
            await _context.SaveChangesAsync();
            return ingreso;
        }
    }
}
