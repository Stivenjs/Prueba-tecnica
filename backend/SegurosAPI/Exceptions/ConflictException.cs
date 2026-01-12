namespace SegurosAPI.Exceptions
{
    /// <summary>
    /// Excepción cuando hay un conflicto con un recurso existente (409 Conflict)
    /// </summary>
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string resourceName, string field, object value) 
            : base($"A {resourceName} with {field} '{value}' already exists")
        {
        }
    }
}
