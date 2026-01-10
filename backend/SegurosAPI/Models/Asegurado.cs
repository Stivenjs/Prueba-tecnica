using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SegurosAPI.Models
{
    public class Asegurado
    {
        /// <summary>
        /// Número de identificación del asegurado (Llave primaria)
        /// </summary>
        [Key]
        [Required(ErrorMessage = "El número de identificación es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "El número de identificación debe ser un valor positivo")]
        public long NumeroIdentificacion { get; set; }

        /// <summary>
        /// Primer nombre del asegurado
        /// </summary>
        [Required(ErrorMessage = "El primer nombre es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El primer nombre debe tener entre 2 y 50 caracteres")]
        public string PrimerNombre { get; set; } = string.Empty;

        /// <summary>
        /// Segundo nombre del asegurado (Opcional)
        /// </summary>
        [StringLength(50, ErrorMessage = "El segundo nombre no puede exceder 50 caracteres")]
        public string? SegundoNombre { get; set; }

        /// <summary>
        /// Primer apellido del asegurado
        /// </summary>
        [Required(ErrorMessage = "El primer apellido es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El primer apellido debe tener entre 2 y 50 caracteres")]
        public string PrimerApellido { get; set; } = string.Empty;

        /// <summary>
        /// Segundo apellido del asegurado
        /// </summary>
        [Required(ErrorMessage = "El segundo apellido es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El segundo apellido debe tener entre 2 y 50 caracteres")]
        public string SegundoApellido { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono de contacto del asegurado
        /// </summary>
        [Required(ErrorMessage = "El teléfono de contacto es requerido")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 20 caracteres")]
        public string TelefonoContacto { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del asegurado
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento del asegurado
        /// </summary>
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Valor estimado de la solicitud del seguro
        /// </summary>
        [Required(ErrorMessage = "El valor estimado de solicitud es requerido")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor estimado debe ser mayor a cero")]
        public decimal ValorEstimadoSolicitud { get; set; }

        /// <summary>
        /// Observaciones adicionales (Opcional)
        /// </summary>
        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime? FechaActualizacion { get; set; }
    }
}
