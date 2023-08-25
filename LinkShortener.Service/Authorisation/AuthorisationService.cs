using LinkShortener.Domain.DTO.Request;
using LinkShortener.Entity.Models;
using LinkShortener.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LinkShortener.Entity.Exceptions;

namespace LinkShortener.Service.Authorisation
{
    public sealed class AuthorisationService
    {
        private readonly IRepositoryBase<User> _repositoryBase;
        private readonly IConfiguration _configuration;

        public AuthorisationService(IConfiguration configuration)
        {
            _repositoryBase = new MSSQLRepository<User>();
            _configuration = configuration;
        }

        public async Task<string> AuthorisateAsync(AuthorisateDataRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login))
            {
                throw new ArgumentNullException(nameof(request.Login), "Login cannot be null or whitespace.");
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentNullException(nameof(request.Password), "Password cannot be null or whitespace.");
            }

            var user = await _repositoryBase.FindAsync(user => user.Login == request.Login) 
                ?? throw new InvalidInputDataException("This login isn't exist.");

            bool isCorrectedPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isCorrectedPassword)
            {
                throw new InvalidInputDataException("Incorrected password.");
            }
            return GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issure"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
