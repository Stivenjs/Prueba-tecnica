using SegurosAPI.DTOs.Requests;
using SegurosAPI.DTOs.Responses;

namespace SegurosAPI.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de Asegurados
    /// Define la lógica de negocio
    /// </summary>
    public interface IAseguradoService
    {
        Task<PagedResponse<AseguradoResponse>> GetAllAsync(int pageNumber, int pageSize);
        Task<AseguradoResponse> GetByIdAsync(long id);
        Task<SearchResponse<AseguradoResponse>> SearchByIdentificationAsync(string numeroIdentificacion);
        Task<AseguradoResponse> CreateAsync(CreateAseguradoRequest request);
        Task<AseguradoResponse> UpdateAsync(long id, UpdateAseguradoRequest request);
        Task DeleteAsync(long id);
    }
}
