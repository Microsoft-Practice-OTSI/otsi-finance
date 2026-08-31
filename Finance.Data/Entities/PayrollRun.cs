using System;
using System.Collections.Generic;
using Finance.Common;

namespace Finance.Data.Entities;

public class PayrollRun
{
    public int Id { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public DateTime RunDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public PayrollRunStatus Status { get; set; } = PayrollRunStatus.Completed;
    public decimal TotalGross { get; set; }
    public decimal TotalNet { get; set; }
    public ICollection<Payslip> Payslips { get; set; } = new List<Payslip>();
}
