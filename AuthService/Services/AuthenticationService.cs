using AuthService.Models;
using AuthService.Models.Auth;
using AuthService.Properties;
using AuthService.Repositories.Interfaces;
using AuthService.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services
{
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<TokenModel> LoginUserAsync(AuthModel authModel)
        {
            var user = await GetByLogin(authModel.Login);

            if (!GetHashedPassword(authModel.Password).Equals(user.Password))
            {
                return null;
            }
            return await CreateTokenAsync(user);
        }

        public async Task<TokenModel> RefreshToken(TokenModel tokenModel)
        {
            var claimsIdentity = await GetPrincipalFromToken(tokenModel.AccessToken);
            if (claimsIdentity == null) throw new Exception("invalid token");

            int.TryParse(claimsIdentity.Name, out var userId);

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RefreshToken != tokenModel.RefreshToken
                || user.RefreshTokenExpireTime <= DateTime.UtcNow)
            {
                throw new Exception("can not refresh token");
            }

            return await CreateTokenAsync(user);
        }

        public async Task<User> UpdateUserRefreshTokenAsync(User user, string refreshToken, DateTime expireDate)
        {
            var dbUser = await GetByLogin(user.Login);
            if (dbUser == null) throw new ArgumentNullException(nameof(dbUser));

            dbUser.RefreshToken = refreshToken;
            dbUser.RefreshTokenExpireTime = expireDate;

            return await _userRepository.UpdateUserRefreshTokenAsync(user);
        }

        public async Task<int> ValidateAccessToken(string token)
        {
            var claimsIdentity = await GetPrincipalFromToken(token);
            if (claimsIdentity == null) throw new Exception("invalid token");

            int.TryParse(claimsIdentity.Name, out var userId);
            return userId;
        }

        private async Task<User> GetByLogin(string login)
        {
            var lowerLogin = login.Trim().ToLower();
            return await _userRepository.GetByLogin(lowerLogin);
        }

        private string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Resources.SecretKey)), SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: Resources.Issuer,
                audience: null,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(30)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<TokenModel> CreateTokenAsync(User user)
        {
            var token = GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            await UpdateUserRefreshTokenAsync(user, refreshToken, DateTime.UtcNow.AddDays(7));

            return new TokenModel { AccessToken = token, RefreshToken = refreshToken };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private async Task<ClaimsIdentity> GetPrincipalFromToken(string token)
        {
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Resources.SecretKey)),
                ValidateLifetime = true,
                ValidIssuer = Resources.Issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = await tokenHandler.ValidateTokenAsync(token, tokenValidationParams);

            if (!principal.IsValid)
            {
                throw new Exception("Invalid token");
            }
            return principal.ClaimsIdentity;
        }
    }
}
