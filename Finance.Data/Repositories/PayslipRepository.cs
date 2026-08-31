using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Finance.Data.Entities;
using Finance.Data.TableStorage;

namespace Finance.Data.Repositories;

public interface IPayslipRepository
{
    Task<List<Payslip>> GetByEmployeeAsync(int employeeId);
    Task<List<Payslip>> GetByRunAsync(int payrollRunId);
    Task AddRangeAsync(IEnumerable<Payslip> payslips);
}

public sealed class PayslipRepository : IPayslipRepository
{
    private readonly IPayrollTableStore _store;
    private readonly TableClient _table;

    public PayslipRepository(IPayrollTableStore store)
    {
        _store = store;
        _table = store.Payslips;
    }

    public async Task<List<Payslip>> GetByEmployeeAsync(int employeeId)
    {
        var employees = await GetEmployeeMapAsync();
        var list = new List<Payslip>();
        await foreach (var t in _table.QueryAsync<PayslipTableEntity>(e => e.EmployeeId == employeeId))
        {
            list.Add(ToDomain(t, employees));
        }

        list.Sort((a, b) => b.PayDate.CompareTo(a.PayDate));
        return list;
    }

    public async Task<List<Payslip>> GetByRunAsync(int payrollRunId)
    {
        var employees = await GetEmployeeMapAsync();
        var list = new List<Payslip>();
        await foreach (var t in _table.QueryAsync<PayslipTableEntity>(e => e.PayrollRunId == payrollRunId))
        {
            list.Add(ToDomain(t, employees));
        }

        list.Sort((a, b) =>
        {
            var employeeA = a.Employee?.LastName ?? string.Empty;
            var employeeB = b.Employee?.LastName ?? string.Empty;
            return string.Compare(employeeA, employeeB, System.StringComparison.OrdinalIgnoreCase);
        });
        return list;
    }

    public async Task AddRangeAsync(IEnumerable<Payslip> payslips)
    {
        foreach (var payslip in payslips)
        {
            var id = await _store.GetNextIdAsync(_table);
            var t = payslip.ToTable();
            t.SetKeys(TableConstants.PayslipPartition, id);
            await _table.AddEntityAsync(t);
        }
    }

    private async Task<Dictionary<int, Employee>> GetEmployeeMapAsync()
    {
        var map = new Dictionary<int, Employee>();
        await foreach (var e in _store.Employees.QueryAsync<EmployeeTableEntity>())
        {
            map[e.Id] = e.ToDomain();
        }

        return map;
    }

    private static Payslip ToDomain(PayslipTableEntity t, Dictionary<int, Employee> employees)
    {
        var payslip = t.ToDomain();
        if (employees.TryGetValue(t.EmployeeId, out var employee))
        {
            payslip.Employee = employee;
        }

        return payslip;
    }
}
