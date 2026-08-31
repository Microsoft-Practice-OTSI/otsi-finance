using System;

namespace Finance.Data.TableStorage;

public sealed class DepartmentTableEntity : PayrollTableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
