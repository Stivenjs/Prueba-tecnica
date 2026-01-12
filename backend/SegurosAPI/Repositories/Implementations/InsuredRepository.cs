using Microsoft.EntityFrameworkCore;
using SegurosAPI.Data;
using SegurosAPI.Models;
using SegurosAPI.Repositories.Interfaces;

namespace SegurosAPI.Repositories.Implementations
{
    /// <summary>
    /// Implementación del repositorio de Asegurados
    /// Maneja el acceso a datos usando Entity Framework
    /// </summary>
    public class InsuredRepository : IInsuredRepository
    {
        private readonly ApplicationDbContext _context;

        public InsuredRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Insured> items, int totalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Insureds.AsQueryable();
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(i => i.IdentificationNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Insured?> GetByIdAsync(long id)
        {
            return await _context.Insureds.FindAsync(id);
        }

        public async Task<IEnumerable<Insured>> SearchByIdentificationAsync(string identificationNumber)
        {
            return await _context.Insureds
                .Where(i => i.IdentificationNumber.ToString().Contains(identificationNumber))
                .OrderBy(i => i.IdentificationNumber)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Insureds.AnyAsync(i => i.IdentificationNumber == id);
        }

        public async Task<bool> ExistsByEmailAsync(string email, long? excludeId = null)
        {
            var query = _context.Insureds.Where(i => i.Email == email);
            
            if (excludeId.HasValue)
            {
                query = query.Where(i => i.IdentificationNumber != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Insured> CreateAsync(Insured insured)
        {
            _context.Insureds.Add(insured);
            await _context.SaveChangesAsync();
            return insured;
        }

        public async Task UpdateAsync(Insured insured)
        {
            _context.Insureds.Update(insured);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Insured insured)
        {
            _context.Insureds.Remove(insured);
            await _context.SaveChangesAsync();
        }
    }
}
