namespace SegurosAPI.Exceptions
{
    /// <summary>
    /// Excepción cuando no se encuentra un recurso
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string entityName, object key) 
            : base($"{entityName} con identificador '{key}' no fue encontrado")
        {
        }
    }
}
