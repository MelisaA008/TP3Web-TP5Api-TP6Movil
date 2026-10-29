using Microsoft.AspNetCore.Mvc;
using TP5.Shared.DTOs;
using TP5WebApi.Services;

namespace TP5WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaService _service;

        public CategoriasController(
            CategoriaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Devuelve todas las categorías.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias =
                await _service.GetAll();

            return Ok(categorias);
        }

        /// <summary>
        /// Devuelve una categoría por Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var categoria =
                await _service.GetById(id);

            if (categoria == null)
            {
                return NotFound(new
                {
                    mensaje =
                        $"No existe una categoría con ID {id}"
                });
            }

            return Ok(categoria);
        }

        /// <summary>
        /// Crea una nueva categoría.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CrearCategoriaDto dto)
        {
            var nuevaCategoria =
                await _service.Create(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = nuevaCategoria.Id },
                nuevaCategoria);
        }

        /// <summary>
        /// Actualiza una categoría existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CrearCategoriaDto dto)
        {
            var actualizado =
                await _service.Update(id, dto);

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje =
                        $"No existe una categoría con ID {id}"
                });
            }

            return Ok(new
            {
                mensaje =
                    "Categoría actualizada correctamente"
            });
        }

        /// <summary>
        /// Elimina una categoría.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var eliminado =
                await _service.Delete(id);

            if (!eliminado)
            {
                return NotFound(new
                {
                    mensaje =
                        $"No existe una categoría con ID {id}"
                });
            }

            return Ok(new
            {
                mensaje =
                    "Categoría eliminada correctamente"
            });
        }
    }
}