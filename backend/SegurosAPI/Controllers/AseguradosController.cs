using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SegurosAPI.Data;
using SegurosAPI.Models;

namespace SegurosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AseguradosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AseguradosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtener lista de asegurados con paginación
        /// </summary>
        /// <param name="pageNumber">Número de página (por defecto 1)</param>
        /// <param name="pageSize">Tamaño de página (por defecto 10)</param>
        /// <returns>Lista paginada de asegurados</returns>
        [HttpGet]
        public async Task<ActionResult<object>> GetAsegurados(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Límite máximo

            var totalRecords = await _context.Asegurados.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var asegurados = await _context.Asegurados
                .OrderBy(a => a.NumeroIdentificacion)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Data = asegurados
            };

            return Ok(response);
        }

        /// <summary>
        /// Obtener un asegurado por su número de identificación
        /// </summary>
        /// <param name="id">Número de identificación del asegurado</param>
        /// <returns>Datos del asegurado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Asegurado>> GetAsegurado(long id)
        {
            var asegurado = await _context.Asegurados.FindAsync(id);

            if (asegurado == null)
            {
                return NotFound(new { message = $"No se encontró un asegurado con el número de identificación {id}" });
            }

            return Ok(asegurado);
        }

        /// <summary>
        /// Buscar asegurados por número de identificación (búsqueda parcial)
        /// </summary>
        /// <param name="numeroIdentificacion">Número de identificación a buscar</param>
        /// <returns>Lista de asegurados que coinciden con la búsqueda</returns>
        [HttpGet("buscar/{numeroIdentificacion}")]
        public async Task<ActionResult<IEnumerable<Asegurado>>> BuscarPorIdentificacion(string numeroIdentificacion)
        {
            if (string.IsNullOrWhiteSpace(numeroIdentificacion))
            {
                return BadRequest(new { message = "Debe proporcionar un número de identificación para buscar" });
            }

            var asegurados = await _context.Asegurados
                .Where(a => a.NumeroIdentificacion.ToString().Contains(numeroIdentificacion))
                .OrderBy(a => a.NumeroIdentificacion)
                .ToListAsync();

            return Ok(asegurados);
        }

        /// <summary>
        /// Crear un nuevo asegurado
        /// </summary>
        /// <param name="asegurado">Datos del asegurado a crear</param>
        /// <returns>Asegurado creado</returns>
        [HttpPost]
        public async Task<ActionResult<Asegurado>> CreateAsegurado(Asegurado asegurado)
        {
            // Validar que el número de identificación no exista
            if (await _context.Asegurados.AnyAsync(a => a.NumeroIdentificacion == asegurado.NumeroIdentificacion))
            {
                return Conflict(new { message = $"Ya existe un asegurado con el número de identificación {asegurado.NumeroIdentificacion}" });
            }

            // Validar que el email no exista
            if (await _context.Asegurados.AnyAsync(a => a.Email == asegurado.Email))
            {
                return Conflict(new { message = $"Ya existe un asegurado con el correo electrónico {asegurado.Email}" });
            }

            // Validar que la fecha de nacimiento no sea futura
            if (asegurado.FechaNacimiento > DateTime.Now)
            {
                return BadRequest(new { message = "La fecha de nacimiento no puede ser una fecha futura" });
            }

            // Validar edad mínima (por ejemplo, 18 años)
            var edad = DateTime.Now.Year - asegurado.FechaNacimiento.Year;
            if (asegurado.FechaNacimiento > DateTime.Now.AddYears(-edad)) edad--;
            
            if (edad < 18)
            {
                return BadRequest(new { message = "El asegurado debe ser mayor de 18 años" });
            }

            asegurado.FechaCreacion = DateTime.UtcNow;
            asegurado.FechaActualizacion = null;

            _context.Asegurados.Add(asegurado);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al guardar el asegurado en la base de datos", error = ex.Message });
            }

            return CreatedAtAction(nameof(GetAsegurado), new { id = asegurado.NumeroIdentificacion }, asegurado);
        }

        /// <summary>
        /// Actualizar un asegurado existente
        /// </summary>
        /// <param name="id">Número de identificación del asegurado</param>
        /// <param name="asegurado">Datos actualizados del asegurado</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsegurado(long id, Asegurado asegurado)
        {
            if (id != asegurado.NumeroIdentificacion)
            {
                return BadRequest(new { message = "El número de identificación no coincide" });
            }

            var aseguradoExistente = await _context.Asegurados.FindAsync(id);
            if (aseguradoExistente == null)
            {
                return NotFound(new { message = $"No se encontró un asegurado con el número de identificación {id}" });
            }

            // Validar que el email no exista en otro registro
            if (await _context.Asegurados.AnyAsync(a => a.Email == asegurado.Email && a.NumeroIdentificacion != id))
            {
                return Conflict(new { message = $"Ya existe otro asegurado con el correo electrónico {asegurado.Email}" });
            }

            // Validar que la fecha de nacimiento no sea futura
            if (asegurado.FechaNacimiento > DateTime.Now)
            {
                return BadRequest(new { message = "La fecha de nacimiento no puede ser una fecha futura" });
            }

            // Validar edad mínima
            var edad = DateTime.Now.Year - asegurado.FechaNacimiento.Year;
            if (asegurado.FechaNacimiento > DateTime.Now.AddYears(-edad)) edad--;
            
            if (edad < 18)
            {
                return BadRequest(new { message = "El asegurado debe ser mayor de 18 años" });
            }

            // Actualizar campos
            aseguradoExistente.PrimerNombre = asegurado.PrimerNombre;
            aseguradoExistente.SegundoNombre = asegurado.SegundoNombre;
            aseguradoExistente.PrimerApellido = asegurado.PrimerApellido;
            aseguradoExistente.SegundoApellido = asegurado.SegundoApellido;
            aseguradoExistente.TelefonoContacto = asegurado.TelefonoContacto;
            aseguradoExistente.Email = asegurado.Email;
            aseguradoExistente.FechaNacimiento = asegurado.FechaNacimiento;
            aseguradoExistente.ValorEstimadoSolicitud = asegurado.ValorEstimadoSolicitud;
            aseguradoExistente.Observaciones = asegurado.Observaciones;
            aseguradoExistente.FechaActualizacion = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AseguradoExists(id))
                {
                    return NotFound(new { message = $"No se encontró un asegurado con el número de identificación {id}" });
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el asegurado en la base de datos", error = ex.Message });
            }

            return Ok(new { message = "Asegurado actualizado exitosamente", data = aseguradoExistente });
        }

        /// <summary>
        /// Eliminar un asegurado
        /// </summary>
        /// <param name="id">Número de identificación del asegurado</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsegurado(long id)
        {
            var asegurado = await _context.Asegurados.FindAsync(id);
            if (asegurado == null)
            {
                return NotFound(new { message = $"No se encontró un asegurado con el número de identificación {id}" });
            }

            _context.Asegurados.Remove(asegurado);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el asegurado de la base de datos", error = ex.Message });
            }

            return Ok(new { message = "Asegurado eliminado exitosamente" });
        }

        private async Task<bool> AseguradoExists(long id)
        {
            return await _context.Asegurados.AnyAsync(e => e.NumeroIdentificacion == id);
        }
    }
}
