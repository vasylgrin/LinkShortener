namespace LinkShortener.Domain.DTO.Request
{
    public sealed class HttpContextRequest
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
    }
}
