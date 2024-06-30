using AuthService.Models;

namespace AuthService.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegisterUserRequest request);
        Task DeleteUserAsync(int userId);
        Task<User> UpdateUserAsync(UpdateUserRequest record);
    }
}
