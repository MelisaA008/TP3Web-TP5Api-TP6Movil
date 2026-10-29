namespace TP5.Shared.DTOs
{
    public class SalidaDto
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public string ProductoNombre { get; set; } = string.Empty;

    public int ClienteId { get; set; }

    public string ClienteNombre { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public DateTime Fecha { get; set; }
}
}