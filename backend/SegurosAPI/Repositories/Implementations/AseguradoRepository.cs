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
    public class AseguradoRepository : IAseguradoRepository
    {
        private readonly ApplicationDbContext _context;

        public AseguradoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Asegurado> items, int totalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Asegurados.AsQueryable();
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.NumeroIdentificacion)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Asegurado?> GetByIdAsync(long id)
        {
            return await _context.Asegurados.FindAsync(id);
        }

        public async Task<IEnumerable<Asegurado>> SearchByIdentificationAsync(string numeroIdentificacion)
        {
            return await _context.Asegurados
                .Where(a => a.NumeroIdentificacion.ToString().Contains(numeroIdentificacion))
                .OrderBy(a => a.NumeroIdentificacion)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Asegurados.AnyAsync(a => a.NumeroIdentificacion == id);
        }

        public async Task<bool> ExistsByEmailAsync(string email, long? excludeId = null)
        {
            var query = _context.Asegurados.Where(a => a.Email == email);
            
            if (excludeId.HasValue)
            {
                query = query.Where(a => a.NumeroIdentificacion != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Asegurado> CreateAsync(Asegurado asegurado)
        {
            _context.Asegurados.Add(asegurado);
            await _context.SaveChangesAsync();
            return asegurado;
        }

        public async Task UpdateAsync(Asegurado asegurado)
        {
            _context.Asegurados.Update(asegurado);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Asegurado asegurado)
        {
            _context.Asegurados.Remove(asegurado);
            await _context.SaveChangesAsync();
        }
    }
}
