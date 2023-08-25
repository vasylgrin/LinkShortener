using LinkShortener.Domain.DTO;
using LinkShortener.Domain.DTO.Request;
using LinkShortener.Entity.Exceptions;
using LinkShortener.Entity.Models;
using LinkShortener.Repository.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkShortener.Service.Authorisation.Tests
{
    [TestClass()]
    public class RegistrationServiceTests
    {
        private string _userLogin;
        private string _userPassword;

        [TestMethod()]
        public async Task RegistrateAsync_UserDataForRegister_NewUser()
        {
            // arrange

            var registrService = new RegistrationService();

            _userLogin = Guid.NewGuid().ToString();
            _userPassword = Guid.NewGuid().ToString();

            var registDataRequest = new RegistrationDataRequest { Login = _userLogin, Password = _userPassword };

            // act

            bool isOk = await registrService.RegistrateAsync(registDataRequest);

            // assert

            if (!isOk)
            {
                Assert.Fail("User wasnt added.");
            }

            await DeleteTestData_Cleanup();
        }

        [TestMethod()]
        public async Task RegistrateAsync_EmptyUserDataForRegister_ArgumentNullException()
        {
            // arrange

            var registrService = new RegistrationService();
            var registDataRequest = new RegistrationDataRequest { Login = "", Password = "" };

            // act
            // assert

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async ()=> await registrService.RegistrateAsync(registDataRequest));
        }

        [TestMethod()]
        public async Task RegistrateAsync_ExistedUserLogin_Exception()
        {
            // arrange

            _userLogin = Guid.NewGuid().ToString();
            _userPassword = Guid.NewGuid().ToString();

            var registDataRequest = new RegistrationDataRequest { Login = _userLogin, Password = _userPassword };

            // act

            bool isOk = await new RegistrationService().RegistrateAsync(registDataRequest);

            // assert

            await Assert.ThrowsExceptionAsync<UserExistException>(async () => await new RegistrationService().RegistrateAsync(registDataRequest));
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