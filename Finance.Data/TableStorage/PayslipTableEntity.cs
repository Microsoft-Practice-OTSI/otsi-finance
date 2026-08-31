using System;

namespace Finance.Data.TableStorage;

public sealed class PayslipTableEntity : PayrollTableEntity
{
    public int PayrollRunId { get; set; }
    public int EmployeeId { get; set; }
    public string RegularPay { get; set; } = "0.00";
    public string OvertimePay { get; set; } = "0.00";
    public string GrossPay { get; set; } = "0.00";
    public string TaxDeduction { get; set; } = "0.00";
    public string OtherDeductions { get; set; } = "0.00";
    public string NetPay { get; set; } = "0.00";
    public DateTime PayDate { get; set; }
}
