using SegurosAPI.DTOs.Requests;
using SegurosAPI.DTOs.Responses;
using SegurosAPI.Exceptions;
using SegurosAPI.Models;
using SegurosAPI.Repositories.Interfaces;
using SegurosAPI.Services.Interfaces;

namespace SegurosAPI.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de Asegurados
    /// Contiene toda la lógica de negocio
    /// </summary>
    public class AseguradoService : IAseguradoService
    {
        private readonly IAseguradoRepository _repository;
        private readonly ILogger<AseguradoService> _logger;

        public AseguradoService(IAseguradoRepository repository, ILogger<AseguradoService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PagedResponse<AseguradoResponse>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var (items, totalCount) = await _repository.GetAllAsync(pageNumber, pageSize);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResponse<AseguradoResponse>
            {
                TotalRecords = totalCount,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Data = items.Select(MapToResponse)
            };
        }

        public async Task<AseguradoResponse> GetByIdAsync(long id)
        {
            var asegurado = await _repository.GetByIdAsync(id);
            
            if (asegurado == null)
            {
                throw new NotFoundException("Asegurado", id);
            }

            return MapToResponse(asegurado);
        }

        public async Task<SearchResponse<AseguradoResponse>> SearchByIdentificationAsync(string numeroIdentificacion)
        {
            if (string.IsNullOrWhiteSpace(numeroIdentificacion))
            {
                throw new BusinessException("Debe proporcionar un número de identificación para buscar");
            }

            var asegurados = await _repository.SearchByIdentificationAsync(numeroIdentificacion);
            var results = asegurados.Select(MapToResponse).ToList();

            var message = results.Count == 0
                ? "No se encontraron asegurados con el número de identificación proporcionado"
                : $"Se encontraron {results.Count} asegurado(s)";

            return new SearchResponse<AseguradoResponse>
            {
                Results = results,
                TotalCount = results.Count,
                SearchTerm = numeroIdentificacion,
                Message = message
            };
        }

        public async Task<AseguradoResponse> CreateAsync(CreateAseguradoRequest request)
        {
            // Validar que no exista
            if (await _repository.ExistsAsync(request.NumeroIdentificacion))
            {
                throw new BusinessException($"Ya existe un asegurado con el número de identificación {request.NumeroIdentificacion}");
            }

            // Validar email único
            if (await _repository.ExistsByEmailAsync(request.Email))
            {
                throw new BusinessException($"Ya existe un asegurado con el correo electrónico {request.Email}");
            }

            // Validar fecha de nacimiento
            ValidateFechaNacimiento(request.FechaNacimiento);

            // Mapear y crear
            var asegurado = new Asegurado
            {
                NumeroIdentificacion = request.NumeroIdentificacion,
                PrimerNombre = request.PrimerNombre,
                SegundoNombre = request.SegundoNombre,
                PrimerApellido = request.PrimerApellido,
                SegundoApellido = request.SegundoApellido,
                TelefonoContacto = request.TelefonoContacto,
                Email = request.Email,
                FechaNacimiento = request.FechaNacimiento,
                ValorEstimadoSolicitud = request.ValorEstimadoSolicitud,
                Observaciones = request.Observaciones,
                FechaCreacion = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(asegurado);
            _logger.LogInformation("Asegurado creado: {NumeroIdentificacion}", created.NumeroIdentificacion);

            return MapToResponse(created);
        }

        public async Task<AseguradoResponse> UpdateAsync(long id, UpdateAseguradoRequest request)
        {
            var asegurado = await _repository.GetByIdAsync(id);
            
            if (asegurado == null)
            {
                throw new NotFoundException("Asegurado", id);
            }

            // Validar email único (excluyendo el actual)
            if (await _repository.ExistsByEmailAsync(request.Email, id))
            {
                throw new BusinessException($"Ya existe otro asegurado con el correo electrónico {request.Email}");
            }

            // Validar fecha de nacimiento
            ValidateFechaNacimiento(request.FechaNacimiento);

            // Actualizar campos
            asegurado.PrimerNombre = request.PrimerNombre;
            asegurado.SegundoNombre = request.SegundoNombre;
            asegurado.PrimerApellido = request.PrimerApellido;
            asegurado.SegundoApellido = request.SegundoApellido;
            asegurado.TelefonoContacto = request.TelefonoContacto;
            asegurado.Email = request.Email;
            asegurado.FechaNacimiento = request.FechaNacimiento;
            asegurado.ValorEstimadoSolicitud = request.ValorEstimadoSolicitud;
            asegurado.Observaciones = request.Observaciones;
            asegurado.FechaActualizacion = DateTime.UtcNow;

            await _repository.UpdateAsync(asegurado);
            _logger.LogInformation("Asegurado actualizado: {NumeroIdentificacion}", id);

            return MapToResponse(asegurado);
        }

        public async Task DeleteAsync(long id)
        {
            var asegurado = await _repository.GetByIdAsync(id);
            
            if (asegurado == null)
            {
                throw new NotFoundException("Asegurado", id);
            }

            await _repository.DeleteAsync(asegurado);
            _logger.LogInformation("Asegurado eliminado: {NumeroIdentificacion}", id);
        }

        #region Métodos Privados

        private static void ValidateFechaNacimiento(DateTime fechaNacimiento)
        {
            if (fechaNacimiento > DateTime.Now)
            {
                throw new BusinessException("La fecha de nacimiento no puede ser una fecha futura");
            }

            var edad = DateTime.Now.Year - fechaNacimiento.Year;
            if (fechaNacimiento > DateTime.Now.AddYears(-edad)) edad--;

            if (edad < 18)
            {
                throw new BusinessException("El asegurado debe ser mayor de 18 años");
            }
        }

        private static AseguradoResponse MapToResponse(Asegurado asegurado)
        {
            return new AseguradoResponse
            {
                NumeroIdentificacion = asegurado.NumeroIdentificacion,
                PrimerNombre = asegurado.PrimerNombre,
                SegundoNombre = asegurado.SegundoNombre,
                PrimerApellido = asegurado.PrimerApellido,
                SegundoApellido = asegurado.SegundoApellido,
                TelefonoContacto = asegurado.TelefonoContacto,
                Email = asegurado.Email,
                FechaNacimiento = asegurado.FechaNacimiento,
                ValorEstimadoSolicitud = asegurado.ValorEstimadoSolicitud,
                Observaciones = asegurado.Observaciones,
                FechaCreacion = asegurado.FechaCreacion,
                FechaActualizacion = asegurado.FechaActualizacion
            };
        }

        #endregion
    }
}
