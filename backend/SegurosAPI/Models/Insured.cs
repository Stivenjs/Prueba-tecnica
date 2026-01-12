using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SegurosAPI.Models
{
    public class Insured
    {
        /// <summary>
        /// Número de identificación del asegurado (Llave primaria)
        /// </summary>
        [Key]
        [Required(ErrorMessage = "Identification number is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Identification number must be a positive value")]
        public long IdentificationNumber { get; set; }

        /// <summary>
        /// Primer nombre del asegurado
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Segundo nombre del asegurado (Opcional)
        /// </summary>
        [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters")]
        public string? MiddleName { get; set; }

        /// <summary>
        /// Primer apellido del asegurado
        /// </summary>
        [Required(ErrorMessage = "First last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First last name must be between 2 and 50 characters")]
        public string FirstLastName { get; set; } = string.Empty;

        /// <summary>
        /// Segundo apellido del asegurado
        /// </summary>
        [Required(ErrorMessage = "Second last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Second last name must be between 2 and 50 characters")]
        public string SecondLastName { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono de contacto del asegurado
        /// </summary>
        [Required(ErrorMessage = "Contact phone is required")]
        [Phone(ErrorMessage = "Phone format is not valid")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Phone must be between 7 and 20 characters")]
        public string ContactPhone { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del asegurado
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email format is not valid")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento del asegurado
        /// </summary>
        [Required(ErrorMessage = "Birth date is required")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Valor estimado de la solicitud del seguro
        /// </summary>
        [Required(ErrorMessage = "Estimated request value is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Estimated value must be greater than zero")]
        public decimal EstimatedRequestValue { get; set; }

        /// <summary>
        /// Observaciones adicionales (Opcional)
        /// </summary>
        [StringLength(500, ErrorMessage = "Observations cannot exceed 500 characters")]
        public string? Observations { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
