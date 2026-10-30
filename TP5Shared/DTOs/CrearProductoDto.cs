namespace TP5Shared.DTOs
{
    public class CrearProductoDto
    {
        public string Nombre { get; set; } = string.Empty;

        public int Stock { get; set; }

        public decimal Precio { get; set; }

        public int CategoriaId { get; set; }

        public int ProveedorId { get; set; }
    }
}