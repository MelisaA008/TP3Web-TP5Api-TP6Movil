using Microsoft.EntityFrameworkCore;
using TP5.Shared.DTOs;
using TP5.Shared.Models;
using TP5WebApi.Data;
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
            RegistrarVentaAsync(
                CrearVentaDto dto)
        {
            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                var cliente =
                    await _context.Clientes
                        .FirstOrDefaultAsync(
                            c => c.Id == dto.ClienteId);

                if (cliente == null)
                {
                    return (
                        null,
                        "Cliente no encontrado");
                }

                if (dto.Detalles == null ||
                    !dto.Detalles.Any())
                {
                    return (
                        null,
                        "La venta debe contener al menos un detalle");
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
                    var producto =
                        await _context.Productos
                            .FirstOrDefaultAsync(
                                p => p.Id == item.ProductoId);

                    if (producto == null)
                    {
                        return (
                            null,
                            $"Producto {item.ProductoId} no encontrado");
                    }

                    if (producto.Stock < item.Cantidad)
                    {
                        return (
                            null,
                            $"Stock insuficiente para {producto.Nombre}");
                    }

                    producto.Stock -= item.Cantidad;

                    var detalle = new DetalleVenta
                    {
                        ProductoId = producto.Id,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = producto.Precio,
                        Producto = producto,
                        Venta = venta
                    };

                    venta.Total +=
                        detalle.Cantidad *
                        detalle.PrecioUnitario;

                    venta.Detalles.Add(detalle);
                }

                await _repository
                    .RegistrarVentaAsync(venta);

                await transaction.CommitAsync();

                _logger.LogInformation(
                    "Venta {VentaId} registrada correctamente",
                    venta.Id);

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
                            ProductoNombre =
                                d.Producto?.Nombre ?? "",
                            Cantidad = d.Cantidad,
                            PrecioUnitario =
                                d.PrecioUnitario,                   
                        })
                        .ToList()
                };

                return (resultado, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _logger.LogError(
                    ex,
                    "Error al registrar venta");

                return (
                    null,
                    "Error interno al registrar la venta");
            }
        }

        public async Task<List<VentaListaDto>>
            GetVentasAsync()
        {
            try
            {
                var ventas =
                    await _repository
                        .ObtenerVentasAsync();

                return ventas
                .Select(v => new VentaListaDto
                {
                    Id = v.Id,
                    ClienteNombre = v.Cliente?.Nombre ?? "",
                    Fecha = v.Fecha,
                    Total = v.Total,

                    // cantidad de renglones
                    CantidadItems = v.Detalles.Count,

                    // cantidad total de unidades vendidas
                    CantidadUnidades =
                    v.Detalles.Sum(d => d.Cantidad)
                })
                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al obtener ventas");

                return [];
            }
        }

        public async Task<VentaDto?>
            GetVentaByIdAsync(int id)
        {
            try
            {
                var venta =
                    await _repository
                        .ObtenerVentaPorIdAsync(id);

                if (venta == null)
                    return null;

                return new VentaDto
                {
                    Id = venta.Id,
                    ClienteId = venta.ClienteId,
                    ClienteNombre =
                        venta.Cliente?.Nombre ?? "",
                    Fecha = venta.Fecha,
                    Total = venta.Total,
                    Detalles = venta.Detalles
                        .Select(d => new DetalleVentaDto
                        {
                            ProductoId =
                                d.ProductoId,
                            ProductoNombre =
                                d.Producto?.Nombre ?? "",
                            Cantidad =
                                d.Cantidad,
                            PrecioUnitario =
                                d.PrecioUnitario,                            
                        })
                        .ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error obteniendo venta {VentaId}",
                    id);

                return null;
            }
        }

        public async Task<(bool Exito, string? Error)>
            EliminarVentaAsync(int id)
        {
            try
            {
                var eliminado =
                    await _repository
                        .EliminarVentaAsync(id);

                if (!eliminado)
                {
                    return (
                        false,
                        "Venta no encontrada");
                }

                return (
                    true,
                    null);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error eliminando venta {VentaId}",
                    id);

                return (
                    false,
                    "Error interno");
            }
        }
    }
}