using Microsoft.EntityFrameworkCore;
using TP5Shared.DTOs;
using TP5Shared.Models;
using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    public class ClienteService
    {
        private readonly DataContext _context;

        public ClienteService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ClienteDto>> GetAll()
        {
            return await _context.Clientes
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre ?? string.Empty,
                    Contacto = c.Contacto ?? string.Empty
                })
                .ToListAsync();
        }

        public async Task<ClienteDto?> GetById(int id)
        {
            return await _context.Clientes
                .Where(c => c.Id == id)
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre ?? string.Empty,
                    Contacto = c.Contacto ?? string.Empty
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Cliente> Create(
            CrearClienteDto dto)
        {
            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                Contacto = dto.Contacto
            };

            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();

            return cliente;
        }

        public async Task<bool> Update(
            int id,
            CrearClienteDto dto)
        {
            var cliente =
                await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return false;

            cliente.Nombre = dto.Nombre;
            cliente.Contacto = dto.Contacto;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var cliente =
                await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return false;

            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<RespuestaPaginadaDto<ClienteDto>> GetAllPaginado(ParametrosPaginacionDto p)
        {
            var query = _context.Clientes.AsQueryable();

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
                .Select(c => new ClienteDto { Id = c.Id,
                    Nombre = c.Nombre ?? string.Empty,
                    Contacto = c.Contacto ?? string.Empty
                })
                .ToListAsync();

            return new RespuestaPaginadaDto<ClienteDto>(datos, total, p.Pagina, p.TamanoPagina);
        }
    }
}