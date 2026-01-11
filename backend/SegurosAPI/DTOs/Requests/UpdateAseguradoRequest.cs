using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SegurosAPI.DTOs.Requests
{
    /// <summary>
    /// DTO para actualizar un asegurado existente
    /// </summary>
    public class UpdateAseguradoRequest
    {
        [Required(ErrorMessage = "El primer nombre es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El primer nombre debe tener entre 2 y 50 caracteres")]
        [SwaggerSchema(Description = "Primer nombre del asegurado")]
        public string PrimerNombre { get; set; } = "Juan";

        [StringLength(50, ErrorMessage = "El segundo nombre no puede exceder 50 caracteres")]
        [SwaggerSchema(Description = "Segundo nombre del asegurado (opcional)")]
        public string? SegundoNombre { get; set; } = "Carlos";

        [Required(ErrorMessage = "El primer apellido es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El primer apellido debe tener entre 2 y 50 caracteres")]
        [SwaggerSchema(Description = "Primer apellido del asegurado")]
        public string PrimerApellido { get; set; } = "Perez";

        [Required(ErrorMessage = "El segundo apellido es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El segundo apellido debe tener entre 2 y 50 caracteres")]
        [SwaggerSchema(Description = "Segundo apellido del asegurado")]
        public string SegundoApellido { get; set; } = "Garcia";

        [Required(ErrorMessage = "El teléfono de contacto es requerido")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 20 caracteres")]
        [SwaggerSchema(Description = "Teléfono de contacto del asegurado")]
        public string TelefonoContacto { get; set; } = "3001234567";

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder 100 caracteres")]
        [SwaggerSchema(Description = "Correo electrónico del asegurado")]
        public string Email { get; set; } = "juan.perez@example.com";

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [SwaggerSchema(Description = "Fecha de nacimiento del asegurado (mayor de 18 años)")]
        public DateTime FechaNacimiento { get; set; } = new DateTime(1990, 5, 15);

        [Required(ErrorMessage = "El valor estimado de solicitud es requerido")]
        [Range(0.01, 999999999999, ErrorMessage = "El valor estimado debe ser mayor a cero")]
        [SwaggerSchema(Description = "Valor estimado de la solicitud del seguro", Format = "decimal")]
        public decimal ValorEstimadoSolicitud { get; set; } = 5000000.50m;

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        [SwaggerSchema(Description = "Observaciones adicionales (opcional)")]
        public string? Observaciones { get; set; } = "Cliente potencial premium";
    }
}
