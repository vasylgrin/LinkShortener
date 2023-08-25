using LinkShortener.Domain.DTO;
using LinkShortener.Domain.DTO.Request;
using LinkShortener.Entity.Exceptions;
using LinkShortener.Entity.Models;
using LinkShortener.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkShortener.Service.Authorisation.Tests
{
    [TestClass()]
    public class AuthorisationServiceTests
    {
        private string _userLogin;
        private string _userPassword;
        private readonly IConfiguration _configuration;

        public AuthorisationServiceTests()
        {
            Dictionary<string, string> initialData = new Dictionary<string, string>
            {
                { "Jwt:Issure", "JWTTest"},
                { "Jwt:Audience", "JWTTest"},
                { "Jwt:Key", "asdfasdfasdfasdfasdfsadfsadfsadfsadfsadfsadf" }

            };
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(initialData).Build();
        }

        [TestMethod()]
        public async Task AuthorisateAsync_ExistUserData_JWTToken()
        {
            // arrange

            await AddNewUserInDb_Initialize();

            var authService = new AuthorisationService(_configuration);
            var authDataRequest = new AuthorisateDataRequest { Login = _userLogin, Password = _userPassword };

            // act 

            string jwtToken = await authService.AuthorisateAsync(authDataRequest);

            // assert

            if (string.IsNullOrWhiteSpace(jwtToken))
            {
                Assert.Fail("JWT token is empty or whitespace.");
            }

            await DeleteTestData_Cleanup();
        }

        [TestMethod()]
        public async Task AuthorisateAsync_NonExistUserData_Exception()
        {
            // arrange
            var authService = new AuthorisationService(_configuration);

            string login = Guid.NewGuid().ToString();
            string password = Guid.NewGuid().ToString();

            var authDataRequest = new AuthorisateDataRequest { Login = login, Password = password };

            // act 
            // assert

            await Assert.ThrowsExceptionAsync<InvalidInputDataException>(async () => await authService.AuthorisateAsync(authDataRequest));
        }

        [TestMethod()]
        public async Task AuthorisateAsync_EmptyinputData_ArgumentNullException()
        {
            // arrange
            var authService = new AuthorisationService(_configuration);
            var authDataRequest = new AuthorisateDataRequest { Login = "", Password = "" };

            // act 
            // assert

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await authService.AuthorisateAsync(authDataRequest));
        }


        public async Task AddNewUserInDb_Initialize()
        {
            var registrService = new RegistrationService();

            _userLogin = Guid.NewGuid().ToString();
            _userPassword = Guid.NewGuid().ToString();

            var registDataRequest = new RegistrationDataRequest { Login = _userLogin, Password = _userPassword };

            bool isOk = await registrService.RegistrateAsync(registDataRequest);
            if (!isOk)
            {
                Assert.Fail();
            }
        }

        public async Task DeleteTestData_Cleanup()
        {
            var repository = new MSSQLRepository<User>();
            var user = await repository.FindAsync(user => user.Login == _userLogin && user.Password == _userPassword);
            if (user != null)
            {
                await repository.RemoveAsync(user);
                return;
            }

            Assert.Fail("Something went wrong while was removing user from db.");
        }
    }
}