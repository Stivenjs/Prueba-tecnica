using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SegurosAPI.DTOs.Requests
{
    /// <summary>
    /// DTO para actualizar un asegurado existente
    /// </summary>
    public class UpdateInsuredRequest
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        [SwaggerSchema(Description = "Primer nombre del asegurado")]
        public string FirstName { get; set; } = "Juan";

        [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters")]
        [SwaggerSchema(Description = "Segundo nombre del asegurado (opcional)")]
        public string? MiddleName { get; set; } = "Carlos";

        [Required(ErrorMessage = "First last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First last name must be between 2 and 50 characters")]
        [SwaggerSchema(Description = "Primer apellido del asegurado")]
        public string FirstLastName { get; set; } = "Perez";

        [Required(ErrorMessage = "Second last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Second last name must be between 2 and 50 characters")]
        [SwaggerSchema(Description = "Segundo apellido del asegurado")]
        public string SecondLastName { get; set; } = "Garcia";

        [Required(ErrorMessage = "Contact phone is required")]
        [Phone(ErrorMessage = "Phone format is not valid")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Phone must be between 7 and 20 characters")]
        [SwaggerSchema(Description = "Teléfono de contacto del asegurado")]
        public string ContactPhone { get; set; } = "3001234567";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email format is not valid")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [SwaggerSchema(Description = "Correo electrónico del asegurado")]
        public string Email { get; set; } = "juan.perez@example.com";

        [Required(ErrorMessage = "Birth date is required")]
        [SwaggerSchema(Description = "Fecha de nacimiento del asegurado (mayor de 18 años)")]
        public DateTime BirthDate { get; set; } = new DateTime(1990, 5, 15);

        [Required(ErrorMessage = "Estimated request value is required")]
        [Range(0.01, 999999999999, ErrorMessage = "Estimated value must be greater than zero")]
        [SwaggerSchema(Description = "Valor estimado de la solicitud del seguro", Format = "decimal")]
        public decimal EstimatedRequestValue { get; set; } = 5000000.50m;

        [StringLength(500, ErrorMessage = "Observations cannot exceed 500 characters")]
        [SwaggerSchema(Description = "Observaciones adicionales (opcional)")]
        public string? Observations { get; set; } = "Cliente potencial premium";
    }
}
