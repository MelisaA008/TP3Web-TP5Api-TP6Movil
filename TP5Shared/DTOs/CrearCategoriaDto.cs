
namespace TP5Shared.DTOs
{
    /// <summary>
    /// Datos mínimos para crear una categoría nueva.
    ///
    /// Se utiliza en los formularios de alta.
    /// </summary>
    public class CrearCategoriaDto
    {
        // Nombre que tendrá la categoría.
        //
        // Ejemplos:
        // Tecnología
        // Librería
        // Alimentos
        public string Nombre { get; set; } = string.Empty;
    }
}