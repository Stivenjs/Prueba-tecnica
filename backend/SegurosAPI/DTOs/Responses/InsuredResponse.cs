namespace SegurosAPI.DTOs.Responses
{
    /// <summary>
    /// DTO de respuesta para un asegurado
    /// </summary>
    public class InsuredResponse
    {
        public long IdentificationNumber { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string FirstLastName { get; set; } = string.Empty;
        public string SecondLastName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public decimal EstimatedRequestValue { get; set; }
        public string? Observations { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
