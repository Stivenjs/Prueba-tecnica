using SegurosAPI.Models;

namespace SegurosAPI.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz del repositorio de Asegurados
    /// Define el contrato para el acceso a datos
    /// </summary>
    public interface IInsuredRepository
    {
        Task<(IEnumerable<Insured> items, int totalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<Insured?> GetByIdAsync(long id);
        Task<IEnumerable<Insured>> SearchByIdentificationAsync(string identificationNumber);
        Task<bool> ExistsAsync(long id);
        Task<bool> ExistsByEmailAsync(string email, long? excludeId = null);
        Task<Insured> CreateAsync(Insured insured);
        Task UpdateAsync(Insured insured);
        Task DeleteAsync(Insured insured);
    }
}
