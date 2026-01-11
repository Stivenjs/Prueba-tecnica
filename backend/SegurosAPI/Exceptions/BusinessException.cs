namespace SegurosAPI.Exceptions
{
    /// <summary>
    /// Excepción de lógica de negocio
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
