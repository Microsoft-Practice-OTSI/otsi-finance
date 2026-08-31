using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Finance.Common;
using Finance.Data.Entities;
using Finance.Data.TableStorage;

namespace Finance.Data.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> ListAsync();
    Task<List<Employee>> GetByDepartmentAsync(int departmentId);
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByEmployeeIdAsync(string employeeId);
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
}

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly IPayrollTableStore _store;
    private readonly TableClient _table;

    public EmployeeRepository(IPayrollTableStore store)
    {
        _store = store;
        _table = store.Employees;
    }

    public async Task<List<Employee>> ListAsync()
    {
        var departments = await GetDepartmentMapAsync();
        var list = new List<Employee>();
        await foreach (var t in _table.QueryAsync<EmployeeTableEntity>())
        {
            list.Add(ToDomain(t, departments));
        }

        list.Sort((a, b) =>
        {
            var c = string.Compare(a.LastName, b.LastName, System.StringComparison.OrdinalIgnoreCase);
            return c != 0 ? c : string.Compare(a.FirstName, b.FirstName, System.StringComparison.OrdinalIgnoreCase);
        });
        return list;
    }

    public async Task<List<Employee>> GetByDepartmentAsync(int departmentId)
    {
        var departments = await GetDepartmentMapAsync();
        var list = new List<Employee>();
        await foreach (var t in _table.QueryAsync<EmployeeTableEntity>(e => e.DepartmentId == departmentId))
        {
            list.Add(ToDomain(t, departments));
        }

        list.Sort((a, b) => string.Compare(a.LastName, b.LastName, System.StringComparison.OrdinalIgnoreCase));
        return list;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        var t = await GetAsync(id);
        if (t is null) return null;
        var departments = await GetDepartmentMapAsync();
        return ToDomain(t, departments);
    }

    public async Task<Employee?> GetByEmployeeIdAsync(string employeeId)
    {
        EmployeeTableEntity? found = null;
        await foreach (var t in _table.QueryAsync<EmployeeTableEntity>(e => e.EmployeeId == employeeId))
        {
            found = t;
            break;
        }

        if (found is null) return null;
        var departments = await GetDepartmentMapAsync();
        return ToDomain(found, departments);
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        var id = await _store.GetNextIdAsync(_table);
        var t = employee.ToTable();
        t.SetKeys(TableConstants.EmployeePartition, id);
        await _table.AddEntityAsync(t);
        var departments = await GetDepartmentMapAsync();
        return ToDomain(t, departments);
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        var t = employee.ToTable();
        t.SetKeys(TableConstants.EmployeePartition, employee.Id);
        await _table.UpsertEntityAsync(t, TableUpdateMode.Replace);
        var departments = await GetDepartmentMapAsync();
        return ToDomain(t, departments);
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            await _table.DeleteEntityAsync(TableConstants.EmployeePartition, TableConstants.RowKey(id), ETag.All);
        }
        catch (RequestFailedException ex) when (ex.Status == 404) { }
    }

    private async Task<EmployeeTableEntity?> GetAsync(int id)
    {
        try
        {
            var response = await _table.GetEntityAsync<EmployeeTableEntity>(TableConstants.EmployeePartition, TableConstants.RowKey(id));
            return response.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404) { return null; }
    }

    private async Task<Dictionary<int, Department>> GetDepartmentMapAsync()
    {
        var map = new Dictionary<int, Department>();
        await foreach (var d in _store.Departments.QueryAsync<DepartmentTableEntity>())
        {
            map[d.Id] = d.ToDomain();
        }

        return map;
    }

    private static Employee ToDomain(EmployeeTableEntity t, Dictionary<int, Department> departments)
    {
        var employee = t.ToDomain();
        if (departments.TryGetValue(t.DepartmentId, out var department))
        {
            employee.Department = department;
        }

        return employee;
    }
}
