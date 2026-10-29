namespace TP5.Shared.DTOs
{
    public class CrearVentaDto
    {
        public int ClienteId { get; set; }

        public List<CrearVentaDetalleDto> Detalles { get; set; } = new();
    }
}