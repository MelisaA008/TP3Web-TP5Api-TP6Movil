using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using TP5Shared.DTOs;
using TP5Shared.Models;

using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    /// <summary>
    /// Lógica de negocio para movimientos
    /// de salida de stock.
    /// </summary>
    public class SalidaService
    {
        private readonly DataContext _context;

        private readonly ILogger<SalidaService> _logger;

        public SalidaService(
            DataContext context,
            ILogger<SalidaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las salidas registradas.
        /// </summary>
        public async Task<List<SalidaDto>>
            GetAll()
        {
            try
            {
                _logger.LogInformation(
                    "Consultando listado completo de salidas.");

                var salidas =
                    await _context
                        .SalidasProductos
                        .Include(s => s.Producto)
                        .Include(s => s.Cliente)
                        .Select(s =>
                            new SalidaDto
                            {
                                Id = s.Id,

                                ProductoId =
                                    s.ProductoId,

                                ProductoNombre =
                                    s.Producto != null
                                        ? s.Producto.Nombre
                                        : string.Empty,

                                ClienteId =
                                    s.ClienteId,

                                ClienteNombre =
                                    s.Cliente != null
                                        ? s.Cliente.Nombre ?? string.Empty
                                        : string.Empty,

                                Cantidad =
                                    s.Cantidad,

                                Fecha =
                                    s.Fecha
                            })
                        .ToListAsync();

                _logger.LogInformation(
                    "Se recuperaron {Cantidad} salidas.",
                    salidas.Count);

                return salidas;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error obteniendo listado de salidas.");

                throw;
            }
        }

        /// <summary>
        /// Registra una salida de stock.
        /// </summary>
        public async Task<SalidaDto?>
            RegistrarSalida(
                CrearSalidaDto dto)
        {
            try
            {
                _logger.LogInformation(
                    "Intento de salida. Producto={ProductoId} Cliente={ClienteId} Cantidad={Cantidad}",
                    dto.ProductoId,
                    dto.ClienteId,
                    dto.Cantidad);

                var producto =
                    await _context.Productos
                        .FirstOrDefaultAsync(
                            p => p.Id ==
                                 dto.ProductoId);

                if (producto == null)
                {
                    _logger.LogWarning(
                        "Salida rechazada. Producto inexistente Id={ProductoId}",
                        dto.ProductoId);

                    return null;
                }

                var cliente =
                    await _context.Clientes
                        .FirstOrDefaultAsync(
                            c => c.Id ==
                                 dto.ClienteId);

                if (cliente == null)
                {
                    _logger.LogWarning(
                        "Salida rechazada. Cliente inexistente Id={ClienteId}",
                        dto.ClienteId);

                    return null;
                }

                if (dto.Cantidad <= 0)
                {
                    _logger.LogWarning(
                        "Salida rechazada. Cantidad inválida {Cantidad}",
                        dto.Cantidad);

                    return null;
                }

                if (producto.Stock < dto.Cantidad)
                {
                    _logger.LogWarning(
                        "Salida rechazada por falta de stock. Producto={ProductoId} StockActual={StockActual} CantidadSolicitada={Cantidad}",
                        producto.Id,
                        producto.Stock,
                        dto.Cantidad);

                    return null;
                }

                var stockAnterior =
                    producto.Stock;

                producto.Stock -=
                    dto.Cantidad;

                var salida =
                    new SalidaProducto
                    {
                        ProductoId =
                            dto.ProductoId,

                        ClienteId =
                            dto.ClienteId,

                        Cantidad =
                            dto.Cantidad,

                        Fecha =
                            DateTime.Now
                    };

                _context.SalidasProductos
                    .Add(salida);

                await _context
                    .SaveChangesAsync();

                _logger.LogInformation(
                    "Salida registrada correctamente. SalidaId={SalidaId} Producto={ProductoId} Cliente={ClienteId} Cantidad={Cantidad} StockAnterior={StockAnterior} StockActual={StockActual}",
                    salida.Id,
                    producto.Id,
                    cliente.Id,
                    dto.Cantidad,
                    stockAnterior,
                    producto.Stock);

                return new SalidaDto
                {
                    Id = salida.Id,

                    ProductoId =
                        salida.ProductoId,

                    ProductoNombre =
                        producto.Nombre
                        ?? string.Empty,

                    ClienteId =
                        salida.ClienteId,

                    ClienteNombre =
                        cliente.Nombre
                        ?? string.Empty,

                    Cantidad =
                        salida.Cantidad,

                    Fecha =
                        salida.Fecha
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error registrando salida. Producto={ProductoId} Cliente={ClienteId}",
                    dto.ProductoId,
                    dto.ClienteId);

                throw;
            }
        }
    }
}