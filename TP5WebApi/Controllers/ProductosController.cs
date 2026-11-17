using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TP5Shared.DTOs;
using TP5WebApi.Services;
using System.IO;

namespace TP5WebApi.Controllers;


    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _service;
        private readonly IWebHostEnvironment _env;
        public ProductosController(ProductoService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<RespuestaPaginadaDto<ProductoDto>>>
        Get( [FromQuery] ParametrosPaginacionDto parametros)
        {
            var productos = await _service.GetAll(parametros);

            return Ok(productos);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            var producto = await _service.GetById(id);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearProductoDto dto)
        {
            var nuevo = await _service.Create(dto);

            return CreatedAtAction(nameof(GetById), new { id = nuevo.Id },nuevo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update( int id, [FromBody] CrearProductoDto dto)
        {
            var actualizado = await _service.Update(id, dto);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.Delete(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/imagenes")]
        public async Task<IActionResult> SubirImagen(int id, IFormFile archivo)
        {
            try
            {
                Console.WriteLine($"[IMG] Inicio subida. Producto={id}");

                if (archivo == null)
                {
                    Console.WriteLine("[IMG] Archivo NULL");
                    return BadRequest("Archivo nulo");
                }

                Console.WriteLine($"[IMG] Nombre={archivo.FileName} " +
                    $"Tamaño={archivo.Length}");

                var producto = await _service.GetEntidad(id);

                if (producto == null)
                {
                    Console.WriteLine($"[IMG] Producto {id} inexistente");
                    return NotFound();
                }

                Console.WriteLine($"[IMG] Producto encontrado: {producto.Nombre}");

                var uploadsPath = Path.Combine(_env.WebRootPath,"uploads");

                Console.WriteLine($"[IMG] UploadPath={uploadsPath}");

                if (!Directory.Exists(uploadsPath))
                {
                    Console.WriteLine("[IMG] Creando carpeta uploads");

                    Directory.CreateDirectory(uploadsPath);
                }

                var extension = Path.GetExtension(archivo.FileName);

                var nombreArchivo = $"{Guid.NewGuid()}{extension}";

                var rutaFisica = Path.Combine(uploadsPath, nombreArchivo);

                Console.WriteLine($"[IMG] Guardando archivo: {rutaFisica}");

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                producto.ImagenPath = $"uploads/{nombreArchivo}";

                Console.WriteLine($"[IMG] ImagenPath={producto.ImagenPath}");

                await _service.GuardarEntidad(producto);

                Console.WriteLine("[IMG] Imagen guardada en base de datos");

                return Ok(new {ruta = producto.ImagenPath});
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR IMG] {ex}");

                return StatusCode(500,ex.Message);
            }
        }
    }