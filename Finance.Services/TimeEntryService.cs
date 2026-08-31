using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Data.Entities;
using Finance.Data.Repositories;
using Finance.Services.Models;

namespace Finance.Services;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository _repository;

    public TimeEntryService(ITimeEntryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TimeEntryModel>> GetByEmployeeAsync(int employeeId, System.DateTime? from = null, System.DateTime? to = null) =>
        (await _repository.GetByEmployeeAsync(employeeId, from, to)).Select(ToModel);

    public async Task<TimeEntryModel> CreateAsync(CreateTimeEntryModel model)
    {
        var entity = new TimeEntry
        {
            EmployeeId = model.EmployeeId,
            EntryDate = model.EntryDate,
            RegularHours = model.RegularHours,
            OvertimeHours = model.OvertimeHours,
            Description = model.Description,
            CreatedDate = System.DateTime.UtcNow
        };
        return ToModel(await _repository.AddAsync(entity));
    }

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);

    private static TimeEntryModel ToModel(TimeEntry t) => new()
    {
        Id = t.Id,
        EmployeeId = t.EmployeeId,
        EntryDate = t.EntryDate,
        RegularHours = t.RegularHours,
        OvertimeHours = t.OvertimeHours,
        Description = t.Description,
        CreatedDate = t.CreatedDate
    };
}
