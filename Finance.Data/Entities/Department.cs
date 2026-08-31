using System;
using System.Collections.Generic;

namespace Finance.Data.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
