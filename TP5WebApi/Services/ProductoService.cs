using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TP5Shared.DTOs;
using TP5Shared.Models;

using TP5WebApi.Repositories;

namespace TP5WebApi.Services;

/// <summary>
/// Lógica de negocio para productos.
/// </summary>
public class ProductoService
{
    private readonly ProductoRepository _repository;
    private readonly ILogger<ProductoService> _logger;
    
    //constructor
    public ProductoService(
           ProductoRepository repository,
           ILogger<ProductoService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Producto?>
    GetEntidad(int id)
    {
        return await _repository
            .GetByIdAsync(id);
    }

    public async Task
    GuardarEntidad(
        Producto producto)
    {
        await _repository
            .UpdateAsync(producto);
    }

    /// <summary>
    /// Obtiene productos aplicando filtros,
    /// ordenamiento y paginación.
    /// </summary>
    public async Task<
    RespuestaPaginadaDto<ProductoDto>>
    GetAll(
        ParametrosPaginacionDto parametros)
    {
        try
        {
            _logger.LogInformation(
                "Consulta productos. Página={Pagina} Tamaño={Tamano} Buscar={Buscar} Categoria={Categoria}",
                parametros.Pagina,
                parametros.TamanoPagina,
                parametros.Buscar,
                parametros.CategoriaId);

            // Consulta base.
            var query = _repository.Query();

            // Buscar por nombre.
            if (!string.IsNullOrWhiteSpace(
                parametros.Buscar))
            {
                var termino =
                    parametros.Buscar
                        .Trim()
                        .ToLower();

                query = query.Where(
                    p =>
                        p.Nombre
                            .ToLower()
                            .Contains(
                                termino));
            }

            // Filtrar por categoría.
            if (
                parametros.CategoriaId.HasValue &&
                parametros.CategoriaId.Value > 0)
            {
                query = query.Where(
                    p =>
                        p.CategoriaId ==
                        parametros.CategoriaId);
            }

            // Ordenar resultados.
            query =
                parametros.OrdenarPor?
                .ToLower()
                switch
                {
                    "nombre" =>
                        query.OrderBy(
                            p => p.Nombre),

                    "nombre_desc" =>
                        query.OrderByDescending(
                            p => p.Nombre),

                    "precio_asc" =>
                        query.OrderBy(
                            p => p.Precio),

                    "precio_desc" =>
                        query.OrderByDescending(
                            p => p.Precio),

                    _ =>
                        query.OrderBy(
                            p => p.Id)
                };

            // Cantidad total encontrada.
            var totalRegistros =
                await query.CountAsync();

            // Aplicar paginación.
            var productos =
                await query
                    .Skip(
                        (parametros.Pagina - 1)
                        *
                        parametros.TamanoPagina)
                    .Take(
                        parametros.TamanoPagina)
                    .Select(
                        p =>
                            new ProductoDto
                            {
                                Id = p.Id,
                                Nombre = p.Nombre,
                                Stock = p.Stock,
                                Precio = p.Precio,
                                ImagenPath = p.ImagenPath,
                                
                                CategoriaId = p.CategoriaId,

                                CategoriaNombre = p.Categoria != null? p.Categoria.Nombre: string.Empty,

                                ProveedorId = p.ProveedorId,

                                ProveedorNombre = p.Proveedor != null? p.Proveedor.Nombre ?? string.Empty: string.Empty}).ToListAsync();
                                _logger.LogInformation("Consulta completada. Total registros={Total}", totalRegistros);

            return new RespuestaPaginadaDto<ProductoDto>
            (
                productos,
                totalRegistros,
                parametros.Pagina,
                parametros.TamanoPagina
            );
        }

        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error obteniendo listado de productos");

            throw;
        }
    }

    /// <summary>
    /// Obtiene un producto por Id.
    /// </summary>
    public async Task<ProductoDto?> GetById(int id)
    {
        try
        {
            _logger.LogInformation(
                "Consulta producto Id={Id}",
                id);

            var producto =
                await _repository.GetByIdAsync(id);

            if (producto == null)
            {
                _logger.LogWarning(
                    "Producto no encontrado Id={Id}",
                    id);

                return null;
            }

            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Stock = producto.Stock,
                Precio = producto.Precio,
                ImagenPath = producto.ImagenPath,
                CategoriaId =
                producto.CategoriaId,

                CategoriaNombre =
                producto.Categoria?.Nombre
                ?? string.Empty,

                ProveedorId =
                producto.ProveedorId,

                ProveedorNombre =
                producto.Proveedor?.Nombre
                ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error obteniendo producto {Id}",
                id);

            throw;
        }
    }

    /// <summary>
    /// Registra un producto nuevo.
    /// </summary>
    public async Task<ProductoDto> Create(
    CrearProductoDto dto)
    {
        try
        {
            _logger.LogInformation(
                "Creando producto {Nombre}",
                dto.Nombre);

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Stock = dto.Stock,
                Precio = dto.Precio,
                CategoriaId = dto.CategoriaId,
                ProveedorId = dto.ProveedorId
            };

            await _repository.AddAsync(producto);

            _logger.LogInformation(
                "Producto creado Id={Id} Nombre={Nombre}",
                producto.Id,
                producto.Nombre);

            return await GetById(producto.Id)
                ?? throw new Exception(
                    "Error al recuperar producto creado");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error creando producto {Nombre}",
                dto.Nombre);

            throw;
        }
    }

    /// <summary>
    /// Actualiza un producto existente.
    /// </summary>
    public async Task<bool> Update(
     int id,
     CrearProductoDto dto)
    {
        try
        {
            _logger.LogInformation(
                "Actualizando producto Id={Id}",
                id);

            var producto =
                await _repository.GetByIdAsync(id);

            if (producto == null)
            {
                _logger.LogWarning(
                    "Producto inexistente Id={Id}",
                    id);

                return false;
            }

            producto.Nombre =
             dto.Nombre;

            producto.Stock =
                dto.Stock;

            producto.Precio =
                dto.Precio;

            producto.CategoriaId =
                dto.CategoriaId;

            producto.ProveedorId =
                dto.ProveedorId;

            await _repository.UpdateAsync(producto);

            _logger.LogInformation(
                "Producto actualizado Id={Id}",
                id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error actualizando producto {Id}",
                id);

            throw;
        }
    }

    /// <summary>
    /// Elimina un producto.
    /// </summary>
    public async Task<bool> Delete(int id)
    {
        try
        {
            _logger.LogInformation(
                "Eliminando producto Id={Id}",
                id);

            var producto =
                await _repository.GetByIdAsync(id);

            if (producto == null)
            {
                _logger.LogWarning(
                    "Producto inexistente Id={Id}",
                    id);

                return false;
            }

            await _repository.DeleteAsync(id);

            _logger.LogInformation(
                "Producto eliminado Id={Id}",
                id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error eliminando producto {Id}",
                id);

            throw;
        }
    }
}