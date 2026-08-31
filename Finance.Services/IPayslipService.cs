using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Services.Models;

namespace Finance.Services;

public interface IPayslipService
{
    Task<IEnumerable<PayslipModel>> GetByEmployeeAsync(int employeeId);
    Task<IEnumerable<PayslipModel>> GetByRunAsync(int payrollRunId);
}
