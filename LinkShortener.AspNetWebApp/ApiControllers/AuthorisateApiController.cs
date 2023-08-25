using LinkShortener.Domain.DTO.Request;
using LinkShortener.Domain.Mapper;
using LinkShortener.Service.Authorisation;
using LinkShortener.Service.UrlShortener;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.AspNetWebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorisateApiController : ControllerBase
    {
        private readonly RegistrationService _registrationService;
        private readonly AuthorisationService _authorisationService;

        public AuthorisateApiController(IConfiguration configuration)
        {
            _registrationService = new RegistrationService();
            _authorisationService = new AuthorisationService(configuration);
        }

        [HttpPost, Route("Authorisate")]
        public async Task<IActionResult> Athorisataion([FromBody] AuthorisateDataRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login))
            {
                return BadRequest("Login cannot be null or whitespace.");
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Login cannot be null or whitespace.");
            }

            var jwt = await _authorisationService.AuthorisateAsync(request);
            if (string.IsNullOrWhiteSpace(jwt))
            {
                return StatusCode(500, "Server error.");
            }

            return Ok(jwt);
        }

        [HttpPost, Route("Registrate")]
        public async Task<IActionResult> Registration([FromBody] RegistrationDataRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login))
            {
                throw new ArgumentNullException(nameof(request.Login), "Login cannot be null or whitespace.");
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentNullException(nameof(request.Password), "Login cannot be null or whitespace.");
            }

            var isSuccessfully = await _registrationService.RegistrateAsync(request);
            if (isSuccessfully)
            {
                return Ok(request);
            }

            return StatusCode(500, "Server error.");
        }

        [HttpPost, Route("createShortUrl")]
        public async Task<IActionResult> CreateShortUrl([FromBody] URLRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                throw new ArgumentNullException(nameof(request.Url), "Url cannot be null or whitespace.");
            }

            HttpContextRequest httpContextRequest = HttpContext.ToHttpContextRequest();

            var shorturl = await new UrlShortenerService().CreateShortUrlAsync(request, httpContextRequest);
            return Ok(shorturl);
        }
    }
}
