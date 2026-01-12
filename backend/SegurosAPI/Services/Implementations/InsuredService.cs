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
    public class InsuredService : IInsuredService
    {
        private readonly IInsuredRepository _repository;
        private readonly ILogger<InsuredService> _logger;

        public InsuredService(IInsuredRepository repository, ILogger<InsuredService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PagedResponse<InsuredResponse>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var (items, totalCount) = await _repository.GetAllAsync(pageNumber, pageSize);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResponse<InsuredResponse>
            {
                TotalRecords = totalCount,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Data = items.Select(MapToResponse)
            };
        }

        public async Task<InsuredResponse> GetByIdAsync(long id)
        {
            var insured = await _repository.GetByIdAsync(id);
            
            if (insured == null)
            {
                throw new NotFoundException("Insured", id);
            }

            return MapToResponse(insured);
        }

        public async Task<SearchResponse<InsuredResponse>> SearchByIdentificationAsync(string identificationNumber)
        {
            if (string.IsNullOrWhiteSpace(identificationNumber))
            {
                throw new BusinessException("You must provide an identification number to search");
            }

            var insureds = await _repository.SearchByIdentificationAsync(identificationNumber);
            var results = insureds.Select(MapToResponse).ToList();

            var message = results.Count == 0
                ? "No insureds found with the provided identification number"
                : $"Found {results.Count} insured(s)";

            return new SearchResponse<InsuredResponse>
            {
                Results = results,
                TotalCount = results.Count,
                SearchTerm = identificationNumber,
                Message = message
            };
        }

        public async Task<InsuredResponse> CreateAsync(CreateInsuredRequest request)
        {
            // Validar que no exista
            if (await _repository.ExistsAsync(request.IdentificationNumber))
            {
                throw new BusinessException($"An insured with identification number {request.IdentificationNumber} already exists");
            }

            // Validar email único
            if (await _repository.ExistsByEmailAsync(request.Email))
            {
                throw new BusinessException($"An insured with email {request.Email} already exists");
            }

            // Validar fecha de nacimiento
            ValidateBirthDate(request.BirthDate);

            // Mapear y crear
            var insured = new Insured
            {
                IdentificationNumber = request.IdentificationNumber,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                FirstLastName = request.FirstLastName,
                SecondLastName = request.SecondLastName,
                ContactPhone = request.ContactPhone,
                Email = request.Email,
                BirthDate = request.BirthDate,
                EstimatedRequestValue = request.EstimatedRequestValue,
                Observations = request.Observations,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(insured);
            _logger.LogInformation("Insured created: {IdentificationNumber}", created.IdentificationNumber);

            return MapToResponse(created);
        }

        public async Task<InsuredResponse> UpdateAsync(long id, UpdateInsuredRequest request)
        {
            var insured = await _repository.GetByIdAsync(id);
            
            if (insured == null)
            {
                throw new NotFoundException("Insured", id);
            }

            // Validar email único (excluyendo el actual)
            if (await _repository.ExistsByEmailAsync(request.Email, id))
            {
                throw new BusinessException($"Another insured with email {request.Email} already exists");
            }

            // Validar fecha de nacimiento
            ValidateBirthDate(request.BirthDate);

            // Actualizar campos
            insured.FirstName = request.FirstName;
            insured.MiddleName = request.MiddleName;
            insured.FirstLastName = request.FirstLastName;
            insured.SecondLastName = request.SecondLastName;
            insured.ContactPhone = request.ContactPhone;
            insured.Email = request.Email;
            insured.BirthDate = request.BirthDate;
            insured.EstimatedRequestValue = request.EstimatedRequestValue;
            insured.Observations = request.Observations;
            insured.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(insured);
            _logger.LogInformation("Insured updated: {IdentificationNumber}", id);

            return MapToResponse(insured);
        }

        public async Task DeleteAsync(long id)
        {
            var insured = await _repository.GetByIdAsync(id);
            
            if (insured == null)
            {
                throw new NotFoundException("Insured", id);
            }

            await _repository.DeleteAsync(insured);
            _logger.LogInformation("Insured deleted: {IdentificationNumber}", id);
        }

        #region Métodos Privados

        private static void ValidateBirthDate(DateTime birthDate)
        {
            if (birthDate > DateTime.Now)
            {
                throw new BusinessException("Birth date cannot be a future date");
            }

            var age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age)) age--;

            if (age < 18)
            {
                throw new BusinessException("Insured must be over 18 years old");
            }
        }

        private static InsuredResponse MapToResponse(Insured insured)
        {
            return new InsuredResponse
            {
                IdentificationNumber = insured.IdentificationNumber,
                FirstName = insured.FirstName,
                MiddleName = insured.MiddleName,
                FirstLastName = insured.FirstLastName,
                SecondLastName = insured.SecondLastName,
                ContactPhone = insured.ContactPhone,
                Email = insured.Email,
                BirthDate = insured.BirthDate,
                EstimatedRequestValue = insured.EstimatedRequestValue,
                Observations = insured.Observations,
                CreatedAt = insured.CreatedAt,
                UpdatedAt = insured.UpdatedAt
            };
        }

        #endregion
    }
}
