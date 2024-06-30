using AuthService.Models;
using AuthService.Repositories.Interfaces;
using AuthService.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AuthService.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> RegisterUserAsync(RegisterUserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var loginExist = await IsLoginExistAsync(request.Login);
            if (loginExist)
            {
                throw new Exception("user with this login already exist");
            }

            var user = new User
            {
                Login = request.Login,
                Email = request.Email,
                Password = GetHashedPassword(request.Password),
            };

            return await _userRepository.RegisterUserAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<User> UpdateUserAsync(UpdateUserRequest request)
        {
            var dbUser = await _userRepository.GetUserByIdAsync(request.Id);
            if (dbUser == null) throw new Exception("user is not exist");

            dbUser.Email = request.Email;
            dbUser.Password = GetHashedPassword(request.Password);
            dbUser.Login = request.Login;

            return await _userRepository.UpdateUserAsync(dbUser);
        }

        private async Task<bool> IsLoginExistAsync(string login)
        {
            var lowerLogin = login.Trim().ToLower();
            return await _userRepository.IsLoginExistAsync(lowerLogin);
        }
    }
}
