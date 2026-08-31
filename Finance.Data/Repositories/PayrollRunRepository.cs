using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Finance.Data.Entities;
using Finance.Data.TableStorage;

namespace Finance.Data.Repositories;

public interface IPayrollRunRepository
{
    Task<List<PayrollRun>> ListAsync();
    Task<PayrollRun?> GetByIdAsync(int id);
    Task<PayrollRun> AddAsync(PayrollRun payrollRun);
}

public sealed class PayrollRunRepository : IPayrollRunRepository
{
    private readonly IPayrollTableStore _store;
    private readonly TableClient _table;

    public PayrollRunRepository(IPayrollTableStore store)
    {
        _store = store;
        _table = store.PayrollRuns;
    }

    public async Task<List<PayrollRun>> ListAsync()
    {
        var list = new List<PayrollRun>();
        await foreach (var t in _table.QueryAsync<PayrollRunTableEntity>())
        {
            list.Add(t.ToDomain());
        }

        list.Sort((a, b) => b.RunDate.CompareTo(a.RunDate));
        return list;
    }

    public async Task<PayrollRun?> GetByIdAsync(int id)
    {
        var t = await GetAsync(id);
        if (t is null) return null;
        var run = t.ToDomain();
        run.Payslips = await GetPayslipsAsync(run.Id);
        return run;
    }

    public async Task<PayrollRun> AddAsync(PayrollRun payrollRun)
    {
        var id = await _store.GetNextIdAsync(_table);
        var t = payrollRun.ToTable();
        t.SetKeys(TableConstants.PayrollRunPartition, id);
        await _table.AddEntityAsync(t);
        var run = t.ToDomain();
        run.Payslips = new List<Payslip>();
        return run;
    }

    private async Task<PayrollRunTableEntity?> GetAsync(int id)
    {
        try
        {
            var response = await _table.GetEntityAsync<PayrollRunTableEntity>(TableConstants.PayrollRunPartition, TableConstants.RowKey(id));
            return response.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404) { return null; }
    }

    private async Task<List<Payslip>> GetPayslipsAsync(int payrollRunId)
    {
        var list = new List<Payslip>();
        await foreach (var p in _store.Payslips.QueryAsync<PayslipTableEntity>(e => e.PayrollRunId == payrollRunId))
        {
            list.Add(p.ToDomain());
        }

        return list;
    }
}
