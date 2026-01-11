namespace SegurosAPI.DTOs.Responses
{
    /// <summary>
    /// DTO de respuesta paginada genérica
    /// </summary>
    public class PagedResponse<T>
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    }
}
