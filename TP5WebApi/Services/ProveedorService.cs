using Microsoft.EntityFrameworkCore;
using TP5Shared.DTOs;
using TP5Shared.Models;
using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    public class ProveedorService
    {
        private readonly DataContext _context;

        public ProveedorService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ProveedorDto>> GetAll()
        {
            return await _context.Proveedores
                .Select(p => new ProveedorDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre ?? string.Empty,  
                    Contacto = p.Contacto ?? string.Empty
                })
                .ToListAsync();
        }

        public async Task<ProveedorDto?> GetById(int id)
        {
            return await _context.Proveedores
                .Where(p => p.Id == id)
                .Select(p => new ProveedorDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre ?? string.Empty,
                    Contacto = p.Contacto ?? string.Empty
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Proveedor> Create(
            CrearProveedorDto dto)
        {
            var proveedor = new Proveedor
            {
                Nombre = dto.Nombre,
                Contacto = dto.Contacto
            };

            _context.Proveedores.Add(proveedor);

            await _context.SaveChangesAsync();

            return proveedor;
        }

        public async Task<bool> Update(
            int id,
            CrearProveedorDto dto)
        {
            var proveedor =
                await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return false;

            proveedor.Nombre = dto.Nombre;
            proveedor.Contacto = dto.Contacto;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var proveedor =
                await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return false;

            _context.Proveedores.Remove(proveedor);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<RespuestaPaginadaDto<ProveedorDto>> GetAllPaginado(ParametrosPaginacionDto p)
        {
            var query = _context.Proveedores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(p.Buscar))
            {
                var termino = p.Buscar.Trim().ToLower();
                query = query.Where(c => (c.Nombre ?? "").ToLower().Contains(termino));
            }

            query = p.OrdenarPor?.ToLower() switch
            {
                "nombre_desc" => query.OrderByDescending(c => c.Nombre),
                "nombre" => query.OrderBy(c => c.Nombre),
                _ => query.OrderBy(c => c.Id)
            };

            var total = await query.CountAsync();

            var datos = await query
                .Skip((p.Pagina - 1) * p.TamanoPagina)
                .Take(p.TamanoPagina)
                .Select(c => new ProveedorDto { Id = c.Id, Nombre = c.Nombre ?? string.Empty, Contacto = c.Contacto ?? string.Empty })
                .ToListAsync();

            return new RespuestaPaginadaDto<ProveedorDto>(datos, total, p.Pagina, p.TamanoPagina);
        }
    }
}