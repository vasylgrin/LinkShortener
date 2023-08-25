namespace LinkShortener.Entity.Exceptions
{
    public sealed class ShortLinkNotFound : Exception
    {
        public object Value { get; set; }
        public ShortLinkNotFound(string message, object value) : base(message)
        {
            Value = value;
        }
    }
}
