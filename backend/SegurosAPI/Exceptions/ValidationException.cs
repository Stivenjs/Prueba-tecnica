namespace SegurosAPI.Exceptions
{
    /// <summary>
    /// Excepción para errores de validación de datos (400 Bad Request)
    /// </summary>
    public class ValidationException : Exception
    {
        public Dictionary<string, List<string>>? Errors { get; set; }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Dictionary<string, List<string>> errors) 
            : base(message)
        {
            Errors = errors;
        }
    }
}
