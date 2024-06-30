using AuthService.Repositories;
using AuthService.Repositories.Interfaces;
using AuthService.Services;
using AuthService.Services.Interfaces;

namespace AuthService.Base
{
    public static class InterfaceBindings
    {
        public static IReadOnlyDictionary<Type, Type> GetServiceBindings()
        {
            var result = new Dictionary<Type, Type>
            {
                { typeof(IUserService), typeof(UserService) },
                { typeof(IAuthenticationService), typeof(AuthenticationService) },
            };
            return result;
        }

        public static IReadOnlyDictionary<Type, Type> GetRepositoryBindings()
        {
            var result = new Dictionary<Type, Type>
            {
                { typeof(IUserRepository), typeof(UserRepository) },
            };
            return result;
        }
    }
}
