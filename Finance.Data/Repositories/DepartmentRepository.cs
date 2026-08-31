using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Finance.Data.Entities;
using Finance.Data.TableStorage;

namespace Finance.Data.Repositories;

public interface IDepartmentRepository
{
    Task<List<Department>> ListAsync();
    Task<Department?> GetByIdAsync(int id);
    Task<Department> AddAsync(Department department);
    Task<Department> UpdateAsync(Department department);
    Task DeleteAsync(int id);
}

public sealed class DepartmentRepository : IDepartmentRepository
{
    private readonly IPayrollTableStore _store;
    private readonly TableClient _table;

    public DepartmentRepository(IPayrollTableStore store)
    {
        _store = store;
        _table = store.Departments;
    }

    public async Task<List<Department>> ListAsync()
    {
        var list = new List<Department>();
        await foreach (var t in _table.QueryAsync<DepartmentTableEntity>())
        {
            list.Add(t.ToDomain());
        }

        list.Sort((a, b) => string.Compare(a.Name, b.Name, System.StringComparison.OrdinalIgnoreCase));
        return list;
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        var t = await GetAsync(id);
        return t is null ? null : t.ToDomain();
    }

    public async Task<Department> AddAsync(Department department)
    {
        var id = await _store.GetNextIdAsync(_table);
        var t = department.ToTable();
        t.SetKeys(TableConstants.DepartmentPartition, id);
        await _table.AddEntityAsync(t);
        return t.ToDomain();
    }

    public async Task<Department> UpdateAsync(Department department)
    {
        var t = department.ToTable();
        t.SetKeys(TableConstants.DepartmentPartition, department.Id);
        await _table.UpsertEntityAsync(t, TableUpdateMode.Replace);
        return t.ToDomain();
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            await _table.DeleteEntityAsync(TableConstants.DepartmentPartition, TableConstants.RowKey(id), ETag.All);
        }
        catch (RequestFailedException ex) when (ex.Status == 404) { }
    }

    private async Task<DepartmentTableEntity?> GetAsync(int id)
    {
        try
        {
            var response = await _table.GetEntityAsync<DepartmentTableEntity>(TableConstants.DepartmentPartition, TableConstants.RowKey(id));
            return response.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404) { return null; }
    }
}
