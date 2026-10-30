using TP5Shared.DTOs;

namespace TP5WebApi.Services
{
    public interface IVentaService
    {
        Task<List<VentaListaDto>> GetVentasAsync();

        Task<(VentaDto? Venta, string? Error)>
            RegistrarVentaAsync(
                CrearVentaDto venta);

        Task<(bool Exito, string? Error)>
            EliminarVentaAsync(
                int id);

        Task<VentaDto?> GetVentaByIdAsync(
            int id);
    }
}