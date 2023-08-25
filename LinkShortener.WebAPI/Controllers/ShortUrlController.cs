using LinkShortener.Domain.DTO.Request;
using LinkShortener.Service.UrlShortener;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortUrlController : Controller
    {
        private readonly UrlShortenerService _urlShortenerService;

        public ShortUrlController()
        {
            _urlShortenerService = new UrlShortenerService();
        }

        [HttpPost, Route("createShortUrl")]
        public async Task<IActionResult> CreateShortUrl([FromBody] URLRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                throw new ArgumentNullException(nameof(request.Url), "Url cannot be null or whitespace.");
            }

            var httpContextRequest = new HttpContextRequest
            {
                Host = HttpContext.Request.Host.Value,
                Scheme = HttpContext.Request.Scheme,
            };

            var shorturl = await _urlShortenerService.CreateShortUrlAsync(request, httpContextRequest);
            return Ok(shorturl);
        }

        [HttpDelete, Route("removeUrl/{id}")]
        public async Task<IActionResult> RemoveUrl(int id)
        {
            var isDeleted = await _urlShortenerService.RemoveShortUrl(id);
            return isDeleted ? Ok() : BadRequest();
        }

        [HttpGet, Route("allShortLink")]
        public async Task<IActionResult> GetAllShortLink()
        {
            var shortsUrl = await _urlShortenerService.GetAllShortUrl();
            return Ok(shortsUrl);
        }
    }
}
