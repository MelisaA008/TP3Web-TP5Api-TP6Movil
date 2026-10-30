using Microsoft.EntityFrameworkCore;
using TP5Shared.DTOs;
using TP5Shared.Models;
using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    public class SalidaService
    {
        private readonly DataContext _context;

        public SalidaService(DataContext context)
        {
            _context = context;
        }

        public async Task<SalidaDto?> RegistrarSalida(
            CrearSalidaDto dto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(
                    p => p.Id == dto.ProductoId);

            if (producto == null)
                return null;

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(
                    c => c.Id == dto.ClienteId);

            if (cliente == null)
                return null;

            if (dto.Cantidad <= 0)
                return null;

            if (producto.Stock < dto.Cantidad)
                return null;

            producto.Stock -= dto.Cantidad;

            var salida = new SalidaProducto
            {
                ProductoId = dto.ProductoId,
                ClienteId = dto.ClienteId,
                Cantidad = dto.Cantidad,
                Fecha = DateTime.Now
            };

            _context.SalidasProductos.Add(salida);

            await _context.SaveChangesAsync();

            return new SalidaDto
            {
                Id = salida.Id,
                ProductoId = salida.ProductoId,
                ProductoNombre = producto.Nombre ?? string.Empty,
                ClienteId = salida.ClienteId,
                ClienteNombre = cliente.Nombre ?? string.Empty,
                Cantidad = salida.Cantidad,
                Fecha = salida.Fecha
            };
        }
    }
}