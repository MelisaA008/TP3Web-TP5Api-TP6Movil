using Microsoft.EntityFrameworkCore;
using TP5Shared.DTOs;
using TP5Shared.Models;
using TP5WebApi.Repositories;

namespace TP5WebApi.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentasRepository _repository;
        private readonly DataContext _context;
        private readonly ILogger<VentaService> _logger;

        public VentaService(
            IVentasRepository repository,
            DataContext context,
            ILogger<VentaService> logger)
        {
            _repository = repository;
            _context = context;
            _logger = logger;
        }

        public async Task<(VentaDto? Venta, string? Error)>
            RegistrarVentaAsync(CrearVentaDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _logger.LogInformation(
                    "Intento de venta. ClienteId={ClienteId} CantidadRenglones={Renglones}",
                    dto.ClienteId, dto.Detalles?.Count ?? 0);

                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.Id == dto.ClienteId);

                if (cliente == null)
                {
                    _logger.LogWarning(
                        "Venta rechazada. Cliente inexistente Id={ClienteId}", dto.ClienteId);

                    return (null, "Cliente no encontrado");
                }

                if (dto.Detalles == null || !dto.Detalles.Any())
                {
                    _logger.LogWarning(
                        "Venta rechazada. Sin detalles. ClienteId={ClienteId}", dto.ClienteId);

                    return (null, "La venta debe contener al menos un detalle");
                }

                var venta = new Venta
                {
                    ClienteId = dto.ClienteId,
                    Fecha = DateTime.Now,
                    Total = 0,
                    Cliente = cliente
                };

                foreach (var item in dto.Detalles)
                {
                    var producto = await _context.Productos
                        .FirstOrDefaultAsync(p => p.Id == item.ProductoId);

                    if (producto == null)
                    {
                        _logger.LogWarning(
                            "Venta rechazada. Producto inexistente Id={ProductoId}", item.ProductoId);

                        return (null, $"Producto {item.ProductoId} no encontrado");
                    }

                    if (producto.Stock < item.Cantidad)
                    {
                        _logger.LogWarning(
                            "Venta rechazada por stock insuficiente. Producto={ProductoId} StockActual={StockActual} Solicitado={Cantidad}",
                            producto.Id, producto.Stock, item.Cantidad);

                        return (null, $"Stock insuficiente para {producto.Nombre}");
                    }

                    var stockAnterior = producto.Stock;
                    producto.Stock -= item.Cantidad;

                    var detalle = new DetalleVenta
                    {
                        ProductoId = producto.Id,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = producto.Precio,
                        Producto = producto,
                        Venta = venta
                    };

                    venta.Total += detalle.Cantidad * detalle.PrecioUnitario;
                    venta.Detalles.Add(detalle);

                    // Registra la salida de stock generada por esta línea de venta,
                    // para que SalidasProductos sea el historial único de salidas.
                    var salida = new SalidaProducto
                    {
                        ProductoId = producto.Id,
                        ClienteId = dto.ClienteId,
                        Cantidad = item.Cantidad,
                        Fecha = venta.Fecha
                    };

                    _context.SalidasProductos.Add(salida);

                    _logger.LogInformation(
                        "Línea de venta procesada. Producto={ProductoId} Cantidad={Cantidad} StockAnterior={StockAnterior} StockActual={StockActual}",
                        producto.Id, item.Cantidad, stockAnterior, producto.Stock);
                }

                await _repository.RegistrarVentaAsync(venta);

                await transaction.CommitAsync();

                _logger.LogInformation(
                    "Venta {VentaId} registrada correctamente. ClienteId={ClienteId} Total={Total}",
                    venta.Id, venta.ClienteId, venta.Total);

                var resultado = new VentaDto
                {
                    Id = venta.Id,
                    ClienteId = cliente.Id,
                    ClienteNombre = cliente.Nombre ?? "",
                    Fecha = venta.Fecha,
                    Total = venta.Total,
                    Detalles = venta.Detalles
                        .Select(d => new DetalleVentaDto
                        {
                            ProductoId = d.ProductoId,
                            ProductoNombre = d.Producto?.Nombre ?? "",
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario,
                        })
                        .ToList()
                };

                return (resultado, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _logger.LogError(ex, "Error al registrar venta. ClienteId={ClienteId}", dto.ClienteId);

                return (null, "Error interno al registrar la venta");
            }
        }

        public async Task<List<VentaListaDto>> GetVentasAsync()
        {
            try
            {
                var ventas = await _repository.ObtenerVentasAsync();

                return ventas
                    .Select(v => new VentaListaDto
                    {
                        Id = v.Id,
                        ClienteNombre = v.Cliente?.Nombre ?? "",
                        Fecha = v.Fecha,
                        Total = v.Total,
                        CantidadItems = v.Detalles.Count,
                        CantidadUnidades = v.Detalles.Sum(d => d.Cantidad)
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ventas");
                return [];
            }
        }

        public async Task<VentaDto?> GetVentaByIdAsync(int id)
        {
            try
            {
                var venta = await _repository.ObtenerVentaPorIdAsync(id);

                if (venta == null)
                {
                    _logger.LogWarning("Consulta de venta inexistente. VentaId={VentaId}", id);
                    return null;
                }

                return new VentaDto
                {
                    Id = venta.Id,
                    ClienteId = venta.ClienteId,
                    ClienteNombre = venta.Cliente?.Nombre ?? "",
                    Fecha = venta.Fecha,
                    Total = venta.Total,
                    Detalles = venta.Detalles
                        .Select(d => new DetalleVentaDto
                        {
                            ProductoId = d.ProductoId,
                            ProductoNombre = d.Producto?.Nombre ?? "",
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario,
                        })
                        .ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo venta {VentaId}", id);
                return null;
            }
        }

        public async Task<(bool Exito, string? Error)> EliminarVentaAsync(int id)
        {
            try
            {
                var eliminado = await _repository.EliminarVentaAsync(id);

                if (!eliminado)
                {
                    _logger.LogWarning("Intento de eliminar venta inexistente. VentaId={VentaId}", id);
                    return (false, "Venta no encontrada");
                }

                _logger.LogInformation("Venta eliminada. VentaId={VentaId}", id);

                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando venta {VentaId}", id);
                return (false, "Error interno");
            }
        }
    }
}