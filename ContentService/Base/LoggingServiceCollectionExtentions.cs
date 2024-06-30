using ContentService.Properties;
using Serilog;
using Serilog.Filters;

namespace ContentService.Base
{
    public static class LoggingServiceCollectionExtentions
    {
        public static IServiceCollection AddServiceLogging(this IServiceCollection services)
        {
            return services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Application", "ContentService")
                .WriteTo.Logger(l => l
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .WriteTo.OpenSearch(Resources.OpenSearchLogs, "logs-{0:yyyy.MM.dd}"))
                .WriteTo.Logger(l => l.WriteTo.Console())
                .CreateLogger()));
        }
    }
}
