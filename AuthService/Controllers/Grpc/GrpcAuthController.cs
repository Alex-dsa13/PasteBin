using AuthService.Models;
using AuthService.Services.Grpc;
using AuthService.Services.Interfaces;
using Grpc.Core;

namespace AuthService.Controllers.Grpc
{
    public class GrpcAuthController : GrpcAuthService.GrpcAuthServiceBase
    {
        private readonly IAuthenticationService _authenticationService;
        public GrpcAuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async override Task<AuthResponse> ValidateToken(AuthRequest authRequest, ServerCallContext context)
        {
            try
            {
                var userId = await _authenticationService.ValidateAccessToken(authRequest.Token);
                return new AuthResponse { UserId = userId };
            }
            catch (Exception ex)
            {
                return new AuthResponse { UserId = 0 };
            }
        }
    }
}
