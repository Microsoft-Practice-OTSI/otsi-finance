namespace Finance.Services.Models;

public class TimeEntryModel
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime EntryDate { get; set; }
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

public class CreateTimeEntryModel
{
    public int EmployeeId { get; set; }
    public DateTime EntryDate { get; set; }
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public string Description { get; set; } = string.Empty;
}
