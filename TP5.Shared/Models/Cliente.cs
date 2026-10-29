namespace TP5.Shared.Models
{
    // -------------------------
    // Clientes
    // -------------------------

    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string? Nombre { get; set; }

        [MaxLength(100)]
        public string? Contacto { get; set; }

        // Relación 1-N con SalidasProductos
        public ICollection<SalidaProducto>? SalidasProductos { get; set; }
    }
}
