namespace TP5Shared.DTOs
{
    public class VentaListaDto
    {
        public int Id { get; set; }

        public string ClienteNombre { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public decimal Total { get; set; }

        public int CantidadItems { get; set; }

        public int CantidadUnidades { get; set; }
    }
}