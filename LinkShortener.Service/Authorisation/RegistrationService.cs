using LinkShortener.Domain.DTO.Request;
using LinkShortener.Domain.Mapper;
using LinkShortener.Entity.Models;
using LinkShortener.Repository.Repositories;
using LinkShortener.Entity.Exceptions;

namespace LinkShortener.Service.Authorisation
{
    public sealed class RegistrationService
    {
        private IRepositoryBase<User> _repositoryBase;
        public RegistrationService()
        {
            _repositoryBase = new MSSQLRepository<User>();
        }


        public async Task<bool> RegistrateAsync(RegistrationDataRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login))
            {
                throw new ArgumentNullException(nameof(request.Login), "Login cannot be null or whitespace.");
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentNullException(nameof(request.Password), "Login cannot be null or whitespace.");
            }

            var user = await _repositoryBase.FindAsync(user => user.Login == request.Login);
            if (user != null)
            {
                throw new UserExistException("User with this login name has alredy existed.", nameof(request.Login));
            }


            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            _repositoryBase.Add(request.ToUser());
            return true;
        }
    }
}
