using Microsoft.AspNetCore.Mvc;
using SegurosAPI.DTOs.Requests;
using SegurosAPI.Services.Interfaces;

namespace SegurosAPI.Controllers
{
    /// <summary>
    /// Controlador de Asegurados
    /// Maneja solo peticiones HTTP, delega lógica de negocio al servicio
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InsuredsController : ControllerBase
    {
        private readonly IInsuredService _service;
        private readonly ILogger<InsuredsController> _logger;

        public InsuredsController(IInsuredService service, ILogger<InsuredsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtener lista de asegurados con paginación
        /// </summary>
        /// <param name="pageNumber">Número de página (por defecto 1)</param>
        /// <param name="pageSize">Tamaño de página (por defecto 10)</param>
        /// <returns>Lista paginada de asegurados</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInsureds(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Obtener un asegurado por su número de identificación
        /// </summary>
        /// <param name="id">Número de identificación del asegurado</param>
        /// <returns>Datos del asegurado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInsured(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Buscar asegurados por número de identificación (búsqueda parcial)
        /// </summary>
        /// <param name="identificationNumber">Número de identificación a buscar</param>
        /// <returns>Resultado de la búsqueda con metadata</returns>
        /// <remarks>
        /// Devuelve un objeto con:
        /// - results: Array de asegurados encontrados
        /// - totalCount: Número total de resultados
        /// - searchTerm: Término de búsqueda usado
        /// - message: Mensaje informativo sobre el resultado
        /// </remarks>
        [HttpGet("search/{identificationNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchByIdentification(string identificationNumber)
        {
            var result = await _service.SearchByIdentificationAsync(identificationNumber);
            return Ok(result);
        }

        /// <summary>
        /// Crear un nuevo asegurado
        /// </summary>
        /// <param name="request">Datos del asegurado a crear</param>
        /// <returns>Asegurado creado</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateInsured([FromBody] CreateInsuredRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetInsured), new { id = result.IdentificationNumber }, result);
        }

        /// <summary>
        /// Actualizar un asegurado existente
        /// </summary>
        /// <param name="id">Número de identificación del asegurado</param>
        /// <param name="request">Datos actualizados del asegurado</param>
        /// <returns>Asegurado actualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInsured(long id, [FromBody] UpdateInsuredRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        /// <summary>
        /// Eliminar un asegurado
        /// </summary>
        /// <param name="id">Número de identificación del asegurado</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteInsured(long id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Insured deleted successfully" });
        }
    }
}
