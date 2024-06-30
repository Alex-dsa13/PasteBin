using AuthService.Models;

namespace AuthService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<User> GetUserByIdAsync(int id);
        Task<User> UpdateUserAsync(User user);
        Task<bool> IsLoginExistAsync(string login);
        Task<User> GetByLogin(string login);
        Task<User> UpdateUserRefreshTokenAsync(User user);
    }
}
