namespace TP5Shared.DTOs
{
    /// <summary>
    /// Parámetros utilizados para paginar, filtrar y ordenar resultados.
    /// Se usa principalmente en listados de productos y otras entidades.
    /// </summary>
    public class ParametrosPaginacionDto
    {
        // Límite máximo permitido por página.
        private const int MaxPageSize = 50;

        private int _pageSize = 10;

        // Página actual a consultar.
        //
        // Ejemplo:
        // Página 1
        // Página 2
        // Página 3
        public int Pagina { get; set; } = 1;

        // Cantidad de registros por página.
        //
        // Se limita automáticamente entre 1 y 50 registros.
        public int TamanoPagina
        {
            get => _pageSize;

            set => _pageSize =
                value > MaxPageSize
                    ? MaxPageSize
                    : value < 1
                        ? 1
                        : value;
        }

        // Texto utilizado para buscar resultados.
        //
        // Ejemplo:
        // "notebook"
        // "mouse"
        // "impresora"
        public string? Buscar { get; set; }

        // Filtro opcional por categoría.
        public int? CategoriaId { get; set; }

        // Campo utilizado para ordenar resultados.
        //
        // Ejemplos:
        // id
        // nombre
        // precio_asc
        // precio_desc
        public string? OrdenarPor { get; set; } = "id";
    }
}
// Este DTO no contiene datos.
//
// Solo define cómo quiero consultar la información:
// - página
// - cantidad de registros
// - búsqueda
// - filtros
// - ordenamiento