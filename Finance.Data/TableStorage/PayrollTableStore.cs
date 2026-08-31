using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Options;

namespace Finance.Data.TableStorage;

public sealed class PayrollTableStore : IPayrollTableStore
{
    private readonly TableServiceClient _serviceClient;

    public PayrollTableStore(IOptions<AzureStorageOptions> options)
    {
        var connectionString = options.Value.ConnectionString;
        IsConfigured = !string.IsNullOrWhiteSpace(connectionString);
        _serviceClient = new TableServiceClient(connectionString);
    }

    public bool IsConfigured { get; }

    public Uri ServiceUri => _serviceClient.Uri;

    public TableClient Departments => _serviceClient.GetTableClient(TableConstants.DepartmentsTable);
    public TableClient Employees => _serviceClient.GetTableClient(TableConstants.EmployeesTable);
    public TableClient TimeEntries => _serviceClient.GetTableClient(TableConstants.TimeEntriesTable);
    public TableClient PayrollRuns => _serviceClient.GetTableClient(TableConstants.PayrollRunsTable);
    public TableClient Payslips => _serviceClient.GetTableClient(TableConstants.PayslipsTable);

    public async Task EnsureTablesExistAsync(CancellationToken cancellationToken = default)
    {
        if (!IsConfigured) return;
        foreach (var table in new[] { Departments, Employees, TimeEntries, PayrollRuns, Payslips })
        {
            await table.CreateIfNotExistsAsync(cancellationToken);
        }
    }

    public async Task<int> GetNextIdAsync(TableClient table, CancellationToken cancellationToken = default)
    {
        var max = 0;
        await foreach (var entity in table.QueryAsync<TableEntity>(select: new[] { "RowKey" }, cancellationToken: cancellationToken))
        {
            if (entity.RowKey.Length == 10 && int.TryParse(entity.RowKey, out var value))
            {
                max = Math.Max(max, value);
            }
        }

        return max + 1;
    }
}
