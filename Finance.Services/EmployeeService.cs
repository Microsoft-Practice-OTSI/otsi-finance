using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finance.Data.Entities;
using Finance.Data.Repositories;
using Finance.Services.Models;

namespace Finance.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EmployeeModel>> GetAllAsync() =>
        (await _repository.ListAsync()).Select(ToModel);

    public async Task<IEnumerable<EmployeeModel>> GetByDepartmentAsync(int departmentId) =>
        (await _repository.GetByDepartmentAsync(departmentId)).Select(ToModel);

    public async Task<EmployeeModel?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : ToModel(entity);
    }

    public async Task<EmployeeModel?> GetByEmployeeIdAsync(string employeeId)
    {
        var entity = await _repository.GetByEmployeeIdAsync(employeeId);
        return entity is null ? null : ToModel(entity);
    }

    public async Task<EmployeeModel> CreateAsync(CreateEmployeeModel model)
    {
        var entity = new Employee
        {
            EmployeeId = model.EmployeeId,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            JobTitle = model.JobTitle,
            DepartmentId = model.DepartmentId,
            PayType = model.PayType,
            AnnualSalary = model.AnnualSalary,
            PayRate = model.PayRate,
            Status = model.Status,
            HireDate = model.HireDate,
            CreatedDate = System.DateTime.UtcNow
        };
        return ToModel(await _repository.AddAsync(entity));
    }

    public async Task<EmployeeModel> UpdateAsync(UpdateEmployeeModel model)
    {
        var entity = await _repository.GetByIdAsync(model.Id)
                     ?? throw new KeyNotFoundException($"Employee {model.Id} not found.");
        entity.EmployeeId = model.EmployeeId;
        entity.FirstName = model.FirstName;
        entity.LastName = model.LastName;
        entity.Email = model.Email;
        entity.JobTitle = model.JobTitle;
        entity.DepartmentId = model.DepartmentId;
        entity.PayType = model.PayType;
        entity.AnnualSalary = model.AnnualSalary;
        entity.PayRate = model.PayRate;
        entity.Status = model.Status;
        entity.HireDate = model.HireDate;
        return ToModel(await _repository.UpdateAsync(entity));
    }

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);

    private static EmployeeModel ToModel(Employee e) => new()
    {
        Id = e.Id,
        EmployeeId = e.EmployeeId,
        FirstName = e.FirstName,
        LastName = e.LastName,
        Email = e.Email,
        JobTitle = e.JobTitle,
        DepartmentId = e.DepartmentId,
        DepartmentName = e.Department?.Name ?? string.Empty,
        PayType = e.PayType,
        AnnualSalary = e.AnnualSalary,
        PayRate = e.PayRate,
        Status = e.Status,
        HireDate = e.HireDate,
        CreatedDate = e.CreatedDate
    };
}
