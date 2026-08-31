using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Services.Models;

namespace Finance.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeModel>> GetAllAsync();
    Task<IEnumerable<EmployeeModel>> GetByDepartmentAsync(int departmentId);
    Task<EmployeeModel?> GetByIdAsync(int id);
    Task<EmployeeModel?> GetByEmployeeIdAsync(string employeeId);
    Task<EmployeeModel> CreateAsync(CreateEmployeeModel model);
    Task<EmployeeModel> UpdateAsync(UpdateEmployeeModel model);
    Task DeleteAsync(int id);
}
