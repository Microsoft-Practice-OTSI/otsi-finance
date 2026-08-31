using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Services.Models;

namespace Finance.Services;

public interface IPayrollService
{
    Task<IEnumerable<PayrollRunModel>> GetRunsAsync();
    Task<PayrollRunModel?> GetRunByIdAsync(int id);
    Task<PayrollRunModel> CreateRunAsync(CreatePayrollRunModel model);
}
