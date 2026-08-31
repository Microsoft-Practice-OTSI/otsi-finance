using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Finance.Services.Web.Health;

public class ConfigurationExistsHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly string _key;

    public ConfigurationExistsHealthCheck(IConfiguration configuration, string key)
    {
        _configuration = configuration;
        _key = key;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var value = _configuration[_key];
        return value is null or { Length: 0 }
            ? Task.FromResult(HealthCheckResult.Unhealthy($"Missing required configuration: {_key}"))
            : Task.FromResult(HealthCheckResult.Healthy($"Configuration present: {_key}"));
    }
}

public static class HealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddConfigurationExistsHealthCheck(this IHealthChecksBuilder builder, IConfiguration configuration, string key)
    {
        return builder.AddCheck($"config:{key}", new ConfigurationExistsHealthCheck(configuration, key));
    }
}
