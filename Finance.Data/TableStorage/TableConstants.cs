using System.Globalization;

namespace Finance.Data.TableStorage;

public static class TableConstants
{
    public static string RowKey(int id) => id.ToString("D10", CultureInfo.InvariantCulture);

    public const string ConnectionStringSection = "AzureStorage:ConnectionString";

    public const string DepartmentsTable = "Departments";
    public const string EmployeesTable = "Employees";
    public const string TimeEntriesTable = "TimeEntries";
    public const string PayrollRunsTable = "PayrollRuns";
    public const string PayslipsTable = "Payslips";

    public const string DepartmentPartition = "DEPARTMENT";
    public const string EmployeePartition = "EMPLOYEE";
    public const string TimeEntryPartition = "TIMEENTRY";
    public const string PayrollRunPartition = "PAYROLLRUN";
    public const string PayslipPartition = "PAYSLIP";
}

