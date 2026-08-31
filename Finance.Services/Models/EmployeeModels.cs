using Finance.Common;

namespace Finance.Services.Models;

public class EmployeeModel
{
    public int Id { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string Email { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public PayType PayType { get; set; }
    public decimal AnnualSalary { get; set; }
    public decimal PayRate { get; set; }
    public EmployeeStatus Status { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateEmployeeModel
{
    public string EmployeeId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public PayType PayType { get; set; }
    public decimal AnnualSalary { get; set; }
    public decimal PayRate { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    public DateTime HireDate { get; set; }
}

public class UpdateEmployeeModel : CreateEmployeeModel
{
    public int Id { get; set; }
}
