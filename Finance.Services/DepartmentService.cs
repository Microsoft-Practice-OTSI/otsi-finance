using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finance.Data.Entities;
using Finance.Data.Repositories;
using Finance.Services.Models;

namespace Finance.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DepartmentModel>> GetAllAsync() =>
        (await _repository.ListAsync()).Select(ToModel);

    public async Task<DepartmentModel?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : ToModel(entity);
    }

    public async Task<DepartmentModel> CreateAsync(CreateDepartmentModel model)
    {
        var entity = new Department
        {
            Name = model.Name,
            Code = model.Code,
            Description = model.Description,
            CreatedDate = System.DateTime.UtcNow
        };
        return ToModel(await _repository.AddAsync(entity));
    }

    public async Task<DepartmentModel> UpdateAsync(int id, CreateDepartmentModel model)
    {
        var entity = await _repository.GetByIdAsync(id)
                     ?? throw new KeyNotFoundException($"Department {id} not found.");
        entity.Name = model.Name;
        entity.Code = model.Code;
        entity.Description = model.Description;
        return ToModel(await _repository.UpdateAsync(entity));
    }

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);

    private static DepartmentModel ToModel(Department d) => new()
    {
        Id = d.Id,
        Name = d.Name,
        Code = d.Code,
        Description = d.Description,
        CreatedDate = d.CreatedDate
    };
}
