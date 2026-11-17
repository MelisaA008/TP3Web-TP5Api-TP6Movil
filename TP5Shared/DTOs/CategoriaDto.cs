namespace TP5Shared.DTOs
{
    /// <summary>
    /// Información básica de una categoría.
    ///
    /// Se utiliza para listados, combos y consultas generales.
    /// </summary>
    public class CategoriaDto
    {
        // Identificador único de la categoría.
        public int Id { get; set; }

        // Nombre de la categoría.
        public string Nombre { get; set; } = string.Empty;
    }
}