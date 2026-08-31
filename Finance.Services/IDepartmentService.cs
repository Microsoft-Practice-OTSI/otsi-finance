using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Services.Models;

namespace Finance.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentModel>> GetAllAsync();
    Task<DepartmentModel?> GetByIdAsync(int id);
    Task<DepartmentModel> CreateAsync(CreateDepartmentModel model);
    Task<DepartmentModel> UpdateAsync(int id, CreateDepartmentModel model);
    Task DeleteAsync(int id);
}
