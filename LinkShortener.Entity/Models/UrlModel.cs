namespace LinkShortener.Entity.Models
{
    public sealed class UrlModel
    {
        public int Id { get; set; }
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
    }
}
