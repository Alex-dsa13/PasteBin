using AuthService.Models;
using AuthService.Models.Auth;

namespace AuthService.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenModel> RefreshToken (TokenModel tokenModel);
        Task<TokenModel> LoginUserAsync(AuthModel authModel);
        Task<User> UpdateUserRefreshTokenAsync(User user, string refreshToken, DateTime expireDate);
        Task<int> ValidateAccessToken(string token);
    }
}
