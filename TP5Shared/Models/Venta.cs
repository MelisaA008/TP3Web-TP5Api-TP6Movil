using System.Text.Json.Serialization;
using TP5Shared.Models;

public class Venta
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Total { get; set; }

    [JsonIgnore]
    public Cliente? Cliente { get; set; }

    [JsonIgnore]
    public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    
}