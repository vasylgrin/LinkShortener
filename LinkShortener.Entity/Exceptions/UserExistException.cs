namespace LinkShortener.Entity.Exceptions
{
    public sealed class UserExistException : Exception
    {
        public string Value { get; }
        public UserExistException(string message, string value) : base(message)
        {
            Value = value;
        }
    }
}
