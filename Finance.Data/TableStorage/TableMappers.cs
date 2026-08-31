using System;
using Finance.Common;
using Finance.Data.Entities;

namespace Finance.Data.TableStorage;

public static class TableMappers
{
    public static DepartmentTableEntity ToTable(this Department d) => new()
    {
        Name = d.Name,
        Code = d.Code,
        Description = d.Description,
        CreatedDate = PayrollTableEntity.ToUtc(d.CreatedDate)
    };

    public static Department ToDomain(this DepartmentTableEntity t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        Code = t.Code,
        Description = t.Description,
        CreatedDate = t.CreatedDate,
        Employees = new System.Collections.Generic.List<Employee>()
    };

    public static EmployeeTableEntity ToTable(this Employee e) => new()
    {
        EmployeeId = e.EmployeeId,
        FirstName = e.FirstName,
        LastName = e.LastName,
        Email = e.Email,
        JobTitle = e.JobTitle,
        DepartmentId = e.DepartmentId,
        PayType = (int)e.PayType,
        AnnualSalary = Money(e.AnnualSalary),
        PayRate = Money(e.PayRate),
        Status = (int)e.Status,
        HireDate = PayrollTableEntity.ToUtc(e.HireDate),
        CreatedDate = PayrollTableEntity.ToUtc(e.CreatedDate)
    };

    public static Employee ToDomain(this EmployeeTableEntity t) => new()
    {
        Id = t.Id,
        EmployeeId = t.EmployeeId,
        FirstName = t.FirstName,
        LastName = t.LastName,
        Email = t.Email,
        JobTitle = t.JobTitle,
        DepartmentId = t.DepartmentId,
        PayType = (PayType)t.PayType,
        AnnualSalary = Money(t.AnnualSalary),
        PayRate = Money(t.PayRate),
        Status = (EmployeeStatus)t.Status,
        HireDate = PayrollTableEntity.ToUtc(t.HireDate),
        CreatedDate = PayrollTableEntity.ToUtc(t.CreatedDate),
        TimeEntries = new System.Collections.Generic.List<TimeEntry>(),
        Payslips = new System.Collections.Generic.List<Payslip>()
    };

    public static TimeEntryTableEntity ToTable(this TimeEntry t) => new()
    {
        EmployeeId = t.EmployeeId,
        EntryDate = PayrollTableEntity.ToUtc(t.EntryDate),
        RegularHours = Money(t.RegularHours),
        OvertimeHours = Money(t.OvertimeHours),
        Description = t.Description,
        CreatedDate = PayrollTableEntity.ToUtc(t.CreatedDate)
    };

    public static TimeEntry ToDomain(this TimeEntryTableEntity t) => new()
    {
        Id = t.Id,
        EmployeeId = t.EmployeeId,
        EntryDate = t.EntryDate,
        RegularHours = Money(t.RegularHours),
        OvertimeHours = Money(t.OvertimeHours),
        Description = t.Description,
        CreatedDate = t.CreatedDate
    };

    public static PayrollRunTableEntity ToTable(this PayrollRun r) => new()
    {
        PeriodStart = PayrollTableEntity.ToUtc(r.PeriodStart),
        PeriodEnd = PayrollTableEntity.ToUtc(r.PeriodEnd),
        RunDate = PayrollTableEntity.ToUtc(r.RunDate),
        CreatedBy = r.CreatedBy,
        Status = (int)r.Status,
        TotalGross = Money(r.TotalGross),
        TotalNet = Money(r.TotalNet)
    };

    public static PayrollRun ToDomain(this PayrollRunTableEntity t) => new()
    {
        Id = t.Id,
        PeriodStart = t.PeriodStart,
        PeriodEnd = t.PeriodEnd,
        RunDate = t.RunDate,
        CreatedBy = t.CreatedBy,
        Status = (PayrollRunStatus)t.Status,
        TotalGross = Money(t.TotalGross),
        TotalNet = Money(t.TotalNet),
        Payslips = new System.Collections.Generic.List<Payslip>()
    };

    public static PayslipTableEntity ToTable(this Payslip p) => new()
    {
        PayrollRunId = p.PayrollRunId,
        EmployeeId = p.EmployeeId,
        RegularPay = Money(p.RegularPay),
        OvertimePay = Money(p.OvertimePay),
        GrossPay = Money(p.GrossPay),
        TaxDeduction = Money(p.TaxDeduction),
        OtherDeductions = Money(p.OtherDeductions),
        NetPay = Money(p.NetPay),
        PayDate = PayrollTableEntity.ToUtc(p.PayDate)
    };

    public static Payslip ToDomain(this PayslipTableEntity t) => new()
    {
        Id = t.Id,
        PayrollRunId = t.PayrollRunId,
        EmployeeId = t.EmployeeId,
        RegularPay = Money(t.RegularPay),
        OvertimePay = Money(t.OvertimePay),
        GrossPay = Money(t.GrossPay),
        TaxDeduction = Money(t.TaxDeduction),
        OtherDeductions = Money(t.OtherDeductions),
        NetPay = Money(t.NetPay),
        PayDate = t.PayDate
    };

    private static string Money(decimal value) => PayrollTableEntity.Money(value);
    private static decimal Money(string? value) => PayrollTableEntity.Money(value);
}
