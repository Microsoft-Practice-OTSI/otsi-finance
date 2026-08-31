using System;
using System.Collections.Generic;
using Finance.Common;

namespace Finance.Data.Entities;

public class Employee
{
    public int Id { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public PayType PayType { get; set; }
    public decimal AnnualSalary { get; set; }
    public decimal PayRate { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    public DateTime HireDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    public ICollection<Payslip> Payslips { get; set; } = new List<Payslip>();
}
