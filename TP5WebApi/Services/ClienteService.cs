using Microsoft.EntityFrameworkCore;
using TP5.Shared.DTOs;
using TP5.Shared.Models;
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
                    Nombre = c.Nombre,
                    Contacto = c.Contacto
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
                    Nombre = c.Nombre,
                    Contacto = c.Contacto
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
    }
}