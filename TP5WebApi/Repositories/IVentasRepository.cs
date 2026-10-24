using TP5WebApi.Models;

namespace TP5WebApi.Repositories
{ 
    public interface IVentasRepository
    {
        Task<Venta> RegistrarVentaAsync(Venta venta);
        Task<List<Venta>> ObtenerVentasAsync();
        Task<Venta?> ObtenerVentaPorIdAsync(int id);
        Task<bool> EliminarVentaAsync(int id);
    }
}