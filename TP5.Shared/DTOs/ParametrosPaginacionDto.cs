namespace TP5.Shared.DTOs
{
    public class ParametrosPaginacionDto
    {
        private const int MaxPageSize = 50;

        private int _pageSize = 10;

        public int Pagina { get; set; } = 1;

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

        public string? Buscar { get; set; }

        public int? CategoriaId { get; set; }

        public string? OrdenarPor { get; set; } = "id";
    }
}