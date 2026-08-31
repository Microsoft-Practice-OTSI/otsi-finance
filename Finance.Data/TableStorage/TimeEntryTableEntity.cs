
namespace Finance.Data.TableStorage;

public sealed class TimeEntryTableEntity : PayrollTableEntity
{
    public int EmployeeId { get; set; }
    public DateTime EntryDate { get; set; }
    public string RegularHours { get; set; } = "0.00";
    public string OvertimeHours { get; set; } = "0.00";
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
