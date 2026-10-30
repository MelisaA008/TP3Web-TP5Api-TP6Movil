using Microsoft.EntityFrameworkCore;
using TP5Shared.DTOs;
using TP5Shared.Models;

namespace TP5WebApi.Services
{
    /// <summary>
    /// Servicio encargado de la lógica de negocio de Productos.
    /// </summary>
    public class ProductoService
    {
        private readonly DataContext _context;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public ProductoService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista paginada de productos.
        /// </summary>
        public async Task<List<ProductoDto>> GetAll(
            int pageNumber = 1,
            int pageSize = 10)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre ?? string.Empty,
                    Stock = p.Stock,
                    Precio = p.Precio,

                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria!.Nombre??string.Empty,

                    ProveedorId = p.ProveedorId,
                    ProveedorNombre = p.Proveedor!.Nombre??string.Empty
                })
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un producto por Id.
        /// </summary>
        public async Task<ProductoDto?> GetById(int id)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Where(p => p.Id == id)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre ?? string.Empty,
                    Stock = p.Stock,
                    Precio = p.Precio,

                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria!.Nombre,

                    ProveedorId = p.ProveedorId,
                    ProveedorNombre = p.Proveedor!.Nombre??string.Empty
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        public async Task<Producto> Create(
            CrearProductoDto dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Stock = dto.Stock,
                Precio = dto.Precio,
                CategoriaId = dto.CategoriaId,
                ProveedorId = dto.ProveedorId
            };

            _context.Productos.Add(producto);

            await _context.SaveChangesAsync();

            return producto;
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        public async Task<bool> Update(
            int id,
            CrearProductoDto dto)
        {
            var producto =
                await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            producto.Nombre = dto.Nombre;
            producto.Stock = dto.Stock;
            producto.Precio = dto.Precio;
            producto.CategoriaId = dto.CategoriaId;
            producto.ProveedorId = dto.ProveedorId;

            _context.Entry(producto).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Elimina un producto por Id.
        /// </summary>
        public async Task<bool> Delete(int id)
        {
            var producto =
                await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
