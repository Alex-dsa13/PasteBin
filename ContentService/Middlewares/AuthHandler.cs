using ContentService.Auth;
using ContentService.Properties;
using ContentService.Services;
using Grpc.Net.Client;

namespace ContentService.Middlewares
{
    public class AuthHandler
    {
        private readonly RequestDelegate _next;
        public AuthHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IIdentityProvider identityProvider)
        {
            var token = context.Request.Cookies["zwpat0_1"];
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancelTokenSource.Token;
            AuthResponse authResponse;

            using (var channel = GrpcChannel.ForAddress(Resources.AuthServiceAddress))
            {
                var client = new GrpcAuthService.GrpcAuthServiceClient(channel);

                authResponse = await client.ValidateTokenAsync(new AuthRequest { Token = token },
                    cancellationToken: cancellationToken);
            }

            if (authResponse == null || authResponse.UserId <= 0)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            identityProvider.Current = new CurrentUser(authResponse.UserId);

            await _next(context);
        }
    }
}
