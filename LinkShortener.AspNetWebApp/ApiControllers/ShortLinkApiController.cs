using LinkShortener.Domain.DTO.Request;
using LinkShortener.Service.UrlShortener;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.AspNetWebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortLinkApiController : Controller
    {
        private readonly UrlShortenerService _urlShortenerService;

        public ShortLinkApiController() { }

        //[HttpGet, Route("shortLink/{shortLink}")]
        //public async Task<IActionResult> RedirectByShortLink(string shortUrl)
        //{
        //    var urlReponse = await _urlShortenerService.FindShortUrlAsync(shortUrl, HttpContext);
        //    var uri = new Uri(urlReponse.LongUrl);

        //    return Redirect(uri.AbsoluteUri);
        //}
    }
}
