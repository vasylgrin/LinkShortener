using LinkShortener.Domain.DTO.Request;
using LinkShortener.Entity.Models;

namespace LinkShortener.Domain.Mapper
{
    public static class UserMapper
    {
        public static User ToUser(this RegistrationDataRequest request)
        {
            return new User
            {
                Login = request.Login,
                Password = request.Password,
            };
        }
    }
}
