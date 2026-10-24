using TP5WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TP5WebApi.Services
{
    public class SalidaService
    {
        private readonly DataContext _context;
        public SalidaService(DataContext context) => _context = context;

        public async Task<SalidaProducto?> RegistrarSalida(SalidaProducto salida)
        {
            var producto = await _context.Productos.FindAsync(salida.ProductoId);
            if (producto == null) return null;

            if (producto.Stock < salida.Cantidad) return null;

            producto.Stock -= salida.Cantidad;
            salida.Fecha = DateTime.Now;

            _context.SalidasProductos.Add(salida);
            await _context.SaveChangesAsync();
            return salida;
        }
    }
}
