using Microsoft.Win32;
using TP5Shared.Models;

namespace TP5Shared.DTOs
{
    /// <summary>
    /// Resultado de una consulta paginada.
    ///
    /// Devuelve los registros solicitados
    /// junto con información necesaria para navegar entre páginas.
    /// </summary>
    public class RespuestaPaginadaDto<T>
    {
        // Página actual devuelta por la consulta.
        public int PaginaActual { get; set; }

        // Cantidad de registros por página.
        public int TamanoPagina { get; set; }

        // Total de registros encontrados.
        public int TotalRegistros { get; set; }

        // Cantidad total de páginas disponibles.
        public int TotalPaginas { get; set; }

        // Indica si existe una página anterior.
        public bool TienePaginaAnterior =>
            PaginaActual > 1;

        // Indica si existe una página siguiente.
        public bool TienePaginaSiguiente =>
            PaginaActual < TotalPaginas;

        // Registros de la página actual.
        public IEnumerable<T> Datos { get; set; }
            = new List<T>();

        public RespuestaPaginadaDto()
        {
        }

        /// <summary>
        /// Construye la respuesta completa calculando automáticamente 
        /// la cantidad total de páginas.
        /// </summary>
        public RespuestaPaginadaDto(
            IEnumerable<T> datos,
            int totalRegistros,
            int paginaActual,
            int tamanoPagina)
        {
            Datos = datos;
            TotalRegistros = totalRegistros;
            PaginaActual = paginaActual;
            TamanoPagina = tamanoPagina;

            TotalPaginas =
                (int)Math.Ceiling(
                    totalRegistros /
                    (double)tamanoPagina);
        }
    }
}
// Este DTO no representa una entidad del sistema.
//
// Se usa para devolver:
//
// - los datos encontrados
// - la página actual
// - la cantidad de páginas
// - el total de registros
//Ejemplo:
//Productos encontrados: 124

//Página actual: 2

//Registros por página: 10

//Resultado:
//TotalPaginas = 13

//Datos:
//Producto 11
//Producto 12
//...
//Producto 20