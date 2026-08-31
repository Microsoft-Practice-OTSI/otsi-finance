using System;

namespace Finance.Data.TableStorage;

public sealed class PayrollRunTableEntity : PayrollTableEntity
{
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public DateTime RunDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public int Status { get; set; }
    public string TotalGross { get; set; } = "0.00";
    public string TotalNet { get; set; } = "0.00";
}
