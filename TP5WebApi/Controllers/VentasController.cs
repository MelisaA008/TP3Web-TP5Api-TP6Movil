using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP5Shared.DTOs;
using TP5WebApi.Services;

namespace TP5WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentasController(
            IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        // POST: api/Ventas
        [HttpPost]
        public async Task<IActionResult> RegistrarVenta(
            [FromBody] CrearVentaDto dto)
        {
            var resultado =
                await _ventaService
                    .RegistrarVentaAsync(dto);

            if (resultado.Error != null)
            {
                return BadRequest(
                    new
                    {
                        error = resultado.Error
                    });
            }

            return Ok(new
            {
                mensaje = "Venta registrada correctamente",
                venta = resultado.Venta
            });
        }

        // GET: api/Ventas
        [HttpGet]
        public async Task<ActionResult<List<VentaListaDto>>>
            GetVentas()
        {
            var ventas =
                await _ventaService.GetVentasAsync();

            return Ok(ventas);
        }

        // GET: api/Ventas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDto>>
            GetVenta(int id)
        {
            var venta =
                await _ventaService
                    .GetVentaByIdAsync(id);

            if (venta == null)
            {
                return NotFound(
                    "Venta no encontrada");
            }

            return Ok(venta);
        }

        // DELETE: api/Ventas/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult>
            EliminarVenta(int id)
        {
            var resultado =
                await _ventaService
                    .EliminarVentaAsync(id);

            if (!resultado.Exito)
            {
                return NotFound(
                    new
                    {
                        error = resultado.Error
                    });
            }

            return Ok(new
            {
                mensaje =
                    "Venta eliminada correctamente"
            });
        }
    }
}