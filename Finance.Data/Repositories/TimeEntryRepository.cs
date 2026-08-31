using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Finance.Data.Entities;
using Finance.Data.TableStorage;

namespace Finance.Data.Repositories;

public interface ITimeEntryRepository
{
    Task<List<TimeEntry>> GetByEmployeeAsync(int employeeId, DateTime? from = null, DateTime? to = null);
    Task<TimeEntry?> GetByIdAsync(int id);
    Task<TimeEntry> AddAsync(TimeEntry timeEntry);
    Task DeleteAsync(int id);
}

public sealed class TimeEntryRepository : ITimeEntryRepository
{
    private readonly IPayrollTableStore _store;
    private readonly TableClient _table;

    public TimeEntryRepository(IPayrollTableStore store)
    {
        _store = store;
        _table = store.TimeEntries;
    }

    public async Task<List<TimeEntry>> GetByEmployeeAsync(int employeeId, DateTime? from = null, DateTime? to = null)
    {
        var list = new List<TimeEntry>();
        await foreach (var t in _table.QueryAsync<TimeEntryTableEntity>(e => e.EmployeeId == employeeId))
        {
            if (from is not null && t.EntryDate < from.Value) continue;
            if (to is not null && t.EntryDate > to.Value) continue;
            list.Add(t.ToDomain());
        }

        list.Sort((a, b) => a.EntryDate.CompareTo(b.EntryDate));
        return list;
    }

    public async Task<TimeEntry?> GetByIdAsync(int id)
    {
        var t = await GetAsync(id);
        return t is null ? null : t.ToDomain();
    }

    public async Task<TimeEntry> AddAsync(TimeEntry timeEntry)
    {
        var id = await _store.GetNextIdAsync(_table);
        var t = timeEntry.ToTable();
        t.SetKeys(TableConstants.TimeEntryPartition, id);
        await _table.AddEntityAsync(t);
        return t.ToDomain();
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            await _table.DeleteEntityAsync(TableConstants.TimeEntryPartition, TableConstants.RowKey(id), ETag.All);
        }
        catch (RequestFailedException ex) when (ex.Status == 404) { }
    }

    private async Task<TimeEntryTableEntity?> GetAsync(int id)
    {
        try
        {
            var response = await _table.GetEntityAsync<TimeEntryTableEntity>(TableConstants.TimeEntryPartition, TableConstants.RowKey(id));
            return response.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404) { return null; }
    }
}
