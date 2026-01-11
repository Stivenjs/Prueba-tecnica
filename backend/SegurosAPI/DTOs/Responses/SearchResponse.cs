namespace SegurosAPI.DTOs.Responses
{
    /// <summary>
    /// Respuesta para búsquedas
    /// </summary>
    public class SearchResponse<T>
    {
        public IEnumerable<T> Results { get; set; } = Enumerable.Empty<T>();
        public int TotalCount { get; set; }
        public string? SearchTerm { get; set; }
        public string? Message { get; set; }
    }
}
