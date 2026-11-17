using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using TP5Shared.DTOs;
using TP5Shared.Models;

using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    public class IngresoService
    {
        private readonly DataContext _context;

        private readonly ILogger<IngresoService> _logger;

        public IngresoService(
            DataContext context,
            ILogger<IngresoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<IngresoDto>>
            GetAll()
        {
            try
            {
                _logger.LogInformation(
                    "Consultando listado completo de ingresos.");

                var ingresos =
                    await _context
                        .IngresosProductos
                        .Include(i => i.Producto)
                        .Include(i => i.Proveedor)
                        .Select(i =>
                            new IngresoDto
                            {
                                Id = i.Id,

                                ProductoId =
                                    i.ProductoId,

                                ProductoNombre =
                                    i.Producto != null
                                        ? i.Producto.Nombre
                                        : string.Empty,

                                ProveedorId =
                                    i.ProveedorId,

                                ProveedorNombre =
                                    i.Proveedor != null
                                        ? i.Proveedor.Nombre ?? string.Empty
                                        : string.Empty,

                                Cantidad =
                                    i.Cantidad,

                                Fecha =
                                    i.Fecha
                            })
                        .ToListAsync();

                _logger.LogInformation(
                    "Se recuperaron {Cantidad} ingresos.",
                    ingresos.Count);

                return ingresos;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error obteniendo listado de ingresos.");

                throw;
            }
        }

        /// <summary>
        /// Registra ingreso de mercadería
        /// y actualiza stock.
        /// </summary>
        public async Task<IngresoDto?> RegistrarIngreso(
            CrearIngresoDto dto)
        {
            try
            {
                _logger.LogInformation(
                    "Intento de ingreso. Producto={ProductoId} Proveedor={ProveedorId} Cantidad={Cantidad}",
                    dto.ProductoId,
                    dto.ProveedorId,
                    dto.Cantidad);

                if (dto.Cantidad <= 0)
                {
                    _logger.LogWarning(
                        "Ingreso rechazado. Cantidad inválida {Cantidad}",
                        dto.Cantidad);

                    return null;
                }

                var producto =
                    await _context.Productos
                        .FirstOrDefaultAsync(
                            p => p.Id == dto.ProductoId);

                if (producto == null)
                {
                    _logger.LogWarning(
                        "Ingreso rechazado. Producto inexistente Id={ProductoId}",
                        dto.ProductoId);

                    return null;
                }

                var proveedor =
                    await _context.Proveedores
                        .FirstOrDefaultAsync(
                            p => p.Id == dto.ProveedorId);

                if (proveedor == null)
                {
                    _logger.LogWarning(
                        "Ingreso rechazado. Proveedor inexistente Id={ProveedorId}",
                        dto.ProveedorId);

                    return null;
                }

                var stockAnterior =
                    producto.Stock;

                producto.Stock += dto.Cantidad;

                var ingreso =
                    new IngresoProducto
                    {
                        ProductoId = dto.ProductoId,
                        ProveedorId = dto.ProveedorId,
                        Cantidad = dto.Cantidad,
                        Fecha = DateTime.Now
                    };

                _context.IngresosProductos.Add(ingreso);

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Ingreso registrado correctamente. IngresoId={IngresoId} Producto={ProductoId} Proveedor={ProveedorId} Cantidad={Cantidad} StockAnterior={StockAnterior} StockActual={StockActual}",
                    ingreso.Id,
                    producto.Id,
                    proveedor.Id,
                    dto.Cantidad,
                    stockAnterior,
                    producto.Stock);

                return new IngresoDto
                {
                    Id = ingreso.Id,

                    ProductoId =
                        ingreso.ProductoId,

                    ProductoNombre =
                        producto.Nombre ?? "",

                    ProveedorId =
                        ingreso.ProveedorId,

                    ProveedorNombre =
                        proveedor.Nombre ?? "",

                    Cantidad =
                        ingreso.Cantidad,

                    Fecha =
                        ingreso.Fecha
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error registrando ingreso. Producto={ProductoId} Proveedor={ProveedorId} Cantidad={Cantidad}",
                    dto.ProductoId,
                    dto.ProveedorId,
                    dto.Cantidad);

                throw;
            }
        }
    }
}