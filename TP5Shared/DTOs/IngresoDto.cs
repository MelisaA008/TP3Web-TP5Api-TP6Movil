namespace TP5Shared.DTOs
{
    public class IngresoDto
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public string ProductoNombre { get; set; }
            = string.Empty;

        public int ProveedorId { get; set; }

        public string ProveedorNombre { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public DateTime Fecha { get; set; }
    }
}
