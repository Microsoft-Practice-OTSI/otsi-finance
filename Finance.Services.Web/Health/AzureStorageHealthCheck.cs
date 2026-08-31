using System.Threading;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Finance.Data.TableStorage;

namespace Finance.Services.Web.Health;

public sealed class AzureStorageHealthCheck : IHealthCheck
{
    private readonly IPayrollTableStore _store;

    public AzureStorageHealthCheck(IPayrollTableStore store)
    {
        _store = store;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (!_store.IsConfigured)
        {
            return HealthCheckResult.Unhealthy("Azure Storage connection string is not configured.");
        }

        try
        {
            var page = _store.Departments.QueryAsync<TableEntity>(select: new[] { "RowKey" }, maxPerPage: 1, cancellationToken: cancellationToken);
            await foreach (var _ in page.WithCancellation(cancellationToken))
            {
                break;
            }

            return HealthCheckResult.Healthy("Azure Table Storage is reachable.");
        }
        catch (System.Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Azure Table Storage is unreachable: {ex.Message}");
        }
    }
}
