namespace ContentService.Middlewares
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        public ErrorHandler(RequestDelegate next)
        {

            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ErrorHandler> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "Error: {message}", ex.Message);
            }
        }
    }
}
