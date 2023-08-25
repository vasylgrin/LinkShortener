namespace LinkShortener.Entity.Exceptions
{
    public sealed class InvalidInputDataException : Exception
    {
        public object[] Values { get; }
        public InvalidInputDataException(string? message, params object[] values) : base(message)
        {
            Values = values;
        }
    }
}
