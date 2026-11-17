using Microsoft.EntityFrameworkCore;
using TP5Shared.DTOs;
using TP5Shared.Models;
using TP5WebApi.Data;

namespace TP5WebApi.Services
{
    public class CategoriaService
    {
        private readonly DataContext _context;

        public CategoriaService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las categorías.
        /// </summary>
        public async Task<List<CategoriaDto>> GetAll()
        {
            return await _context.Categorias
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                })
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una categoría por Id.
        /// </summary>
        public async Task<CategoriaDto?> GetById(int id)
        {
            return await _context.Categorias
                .Where(c => c.Id == id)
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Crea una nueva categoría.
        /// </summary>
        public async Task<Categoria> Create(
            CrearCategoriaDto dto)
        {
            var categoria = new Categoria
            {
                Nombre = dto.Nombre
            };

            _context.Categorias.Add(categoria);

            await _context.SaveChangesAsync();

            return categoria;
        }

        /// <summary>
        /// Modifica una categoría existente.
        /// </summary>
        public async Task<bool> Update(
            int id,
            CrearCategoriaDto dto)
        {
            var categoria =
                await _context.Categorias
                    .FindAsync(id);

            if (categoria == null)
                return false;

            categoria.Nombre = dto.Nombre;

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Elimina una categoría.
        /// </summary>
        public async Task<bool> Delete(int id)
        {
            var categoria =
                await _context.Categorias
                    .FindAsync(id);

            if (categoria == null)
                return false;

            _context.Categorias.Remove(categoria);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<RespuestaPaginadaDto<CategoriaDto>> GetAllPaginado(ParametrosPaginacionDto p)
        {
            var query = _context.Categorias.AsQueryable();

            if (!string.IsNullOrWhiteSpace(p.Buscar))
            {
                var termino = p.Buscar.Trim().ToLower();
                query = query.Where(c => c.Nombre.ToLower().Contains(termino));
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
                .Select(c => new CategoriaDto { Id = c.Id, Nombre = c.Nombre })
                .ToListAsync();

            return new RespuestaPaginadaDto<CategoriaDto>(datos, total, p.Pagina, p.TamanoPagina);
        }
    }
}