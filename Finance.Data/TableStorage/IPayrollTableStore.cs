using System.Threading;
using System.Threading.Tasks;
using Azure.Data.Tables;

namespace Finance.Data.TableStorage;

public interface IPayrollTableStore
{
    TableClient Departments { get; }
    TableClient Employees { get; }
    TableClient TimeEntries { get; }
    TableClient PayrollRuns { get; }
    TableClient Payslips { get; }
    Uri ServiceUri { get; }
    bool IsConfigured { get; }
    Task EnsureTablesExistAsync(CancellationToken cancellationToken = default);
    Task<int> GetNextIdAsync(TableClient table, CancellationToken cancellationToken = default);
}
