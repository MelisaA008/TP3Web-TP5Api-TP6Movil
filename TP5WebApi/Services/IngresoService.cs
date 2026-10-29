using Microsoft.EntityFrameworkCore;
using TP5.Shared.DTOs;
using TP5.Shared.Models;
using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    public class IngresoService
    {
        private readonly DataContext _context;

        public IngresoService(DataContext context)
        {
            _context = context;
        }

        public async Task<IngresoDto?> RegistrarIngreso(
            CrearIngresoDto dto)
        {
            var producto =
                await _context.Productos
                    .FirstOrDefaultAsync(
                        p => p.Id == dto.ProductoId);

            if (producto == null)
                return null;

            producto.Stock += dto.Cantidad;

            var ingreso = new IngresoProducto
            {
                ProductoId = dto.ProductoId,
                ProveedorId = dto.ProveedorId,
                Cantidad = dto.Cantidad,
                Fecha = DateTime.Now
            };

            _context.IngresosProductos.Add(ingreso);

            await _context.SaveChangesAsync();

            var proveedor =
                await _context.Proveedores
                    .FirstOrDefaultAsync(
                        p => p.Id == dto.ProveedorId);

            return new IngresoDto
            {
                Id = ingreso.Id,
                ProductoId = ingreso.ProductoId,
                ProductoNombre = producto.Nombre ?? "",
                ProveedorId = ingreso.ProveedorId,
                ProveedorNombre = proveedor?.Nombre ?? "",
                Cantidad = ingreso.Cantidad,
                Fecha = ingreso.Fecha
            };
        }
    }
}