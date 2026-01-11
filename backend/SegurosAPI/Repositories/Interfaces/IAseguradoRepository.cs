using SegurosAPI.Models;

namespace SegurosAPI.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz del repositorio de Asegurados
    /// Define el contrato para el acceso a datos
    /// </summary>
    public interface IAseguradoRepository
    {
        Task<(IEnumerable<Asegurado> items, int totalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<Asegurado?> GetByIdAsync(long id);
        Task<IEnumerable<Asegurado>> SearchByIdentificationAsync(string numeroIdentificacion);
        Task<bool> ExistsAsync(long id);
        Task<bool> ExistsByEmailAsync(string email, long? excludeId = null);
        Task<Asegurado> CreateAsync(Asegurado asegurado);
        Task UpdateAsync(Asegurado asegurado);
        Task DeleteAsync(Asegurado asegurado);
    }
}
