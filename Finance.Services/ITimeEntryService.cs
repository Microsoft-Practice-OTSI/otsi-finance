using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Services.Models;

namespace Finance.Services;

public interface ITimeEntryService
{
    Task<IEnumerable<TimeEntryModel>> GetByEmployeeAsync(int employeeId, DateTime? from = null, DateTime? to = null);
    Task<TimeEntryModel> CreateAsync(CreateTimeEntryModel model);
    Task DeleteAsync(int id);
}
