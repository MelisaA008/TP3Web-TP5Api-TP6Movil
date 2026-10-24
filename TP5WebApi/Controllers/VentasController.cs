using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP5WebApi.Models;
using TP5WebApi.Services;

namespace TP5WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly VentaService _ventaService;

        public VentasController(VentaService ventaService)
        {
            _ventaService = ventaService;
        }

        // POST: api/Ventas
        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] Venta venta)
        {
            try
            {
                var nuevaVenta = await _ventaService.RegistrarVentaAsync(venta);
                return Ok(new
                {
                    mensaje = "Venta registrada correctamente",
                    venta = nuevaVenta
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/Ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            var ventas = await _ventaService.ObtenerVentasAsync();
            return Ok(ventas);
        }

        // GET: api/Ventas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            var venta = await _ventaService.ObtenerVentaPorIdAsync(id);
            if (venta == null) return NotFound("Venta no encontrada");
            return Ok(venta);
        }

        // DELETE: api/Ventas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarVenta(int id)
        {
            try
            {
                var eliminado = await _ventaService.EliminarVentaAsync(id);
                if (!eliminado) return NotFound("Venta no encontrada");
                return Ok(new { mensaje = "Venta eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
