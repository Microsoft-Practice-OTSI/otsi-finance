using Finance.Common;

namespace Finance.Services.Models;

public class PayrollRunModel
{
    public int Id { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public DateTime RunDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public PayrollRunStatus Status { get; set; }
    public decimal TotalGross { get; set; }
    public decimal TotalNet { get; set; }
    public int PayslipCount { get; set; }
}

public class CreatePayrollRunModel
{
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public DateTime PayDate { get; set; }
    public int? DepartmentId { get; set; }
}

public class PayslipModel
{
    public int Id { get; set; }
    public int EmployeeKey { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public decimal RegularPay { get; set; }
    public decimal OvertimePay { get; set; }
    public decimal GrossPay { get; set; }
    public decimal TaxDeduction { get; set; }
    public decimal OtherDeductions { get; set; }
    public decimal NetPay { get; set; }
    public DateTime PayDate { get; set; }
}
