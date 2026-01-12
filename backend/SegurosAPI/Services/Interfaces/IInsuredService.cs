using SegurosAPI.DTOs.Requests;
using SegurosAPI.DTOs.Responses;

namespace SegurosAPI.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de Asegurados
    /// Define la lógica de negocio
    /// </summary>
    public interface IInsuredService
    {
        Task<PagedResponse<InsuredResponse>> GetAllAsync(int pageNumber, int pageSize);
        Task<InsuredResponse> GetByIdAsync(long id);
        Task<SearchResponse<InsuredResponse>> SearchByIdentificationAsync(string identificationNumber);
        Task<InsuredResponse> CreateAsync(CreateInsuredRequest request);
        Task<InsuredResponse> UpdateAsync(long id, UpdateInsuredRequest request);
        Task DeleteAsync(long id);
    }
}
