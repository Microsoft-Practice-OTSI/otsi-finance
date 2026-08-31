using System;

namespace Finance.Data.TableStorage;

public sealed class EmployeeTableEntity : PayrollTableEntity
{
    public string EmployeeId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int PayType { get; set; }
    public string AnnualSalary { get; set; } = "0.00";
    public string PayRate { get; set; } = "0.00";
    public int Status { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime CreatedDate { get; set; }
}
