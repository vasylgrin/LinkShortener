using LinkShortener.Domain.DTO.Response;
using LinkShortener.Entity.Models;

namespace LinkShortener.Domain.Mapper
{
    public static class URLMapper
    {
        public static URLResponse ToUrlResponse(this UrlModel urlModel)
        {
            return new URLResponse { LongUrl = urlModel.LongUrl, ShortUrl = urlModel.ShortUrl };
        }
    }
}
