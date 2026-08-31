using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Finance.Common;
using Finance.Data.Entities;
using Finance.Data.Repositories;
using Finance.Services.Models;

namespace Finance.Services;

public class PayrollService : IPayrollService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITimeEntryRepository _timeEntryRepository;
    private readonly IPayrollRunRepository _payrollRunRepository;
    private readonly IPayslipRepository _payslipRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUsernameProvider _usernameProvider;
    private readonly PayrollOptions _options;

    public PayrollService(
        IEmployeeRepository employeeRepository,
        ITimeEntryRepository timeEntryRepository,
        IPayrollRunRepository payrollRunRepository,
        IPayslipRepository payslipRepository,
        IDateTimeProvider dateTimeProvider,
        IUsernameProvider usernameProvider,
        IOptions<PayrollOptions> options)
    {
        _employeeRepository = employeeRepository;
        _timeEntryRepository = timeEntryRepository;
        _payrollRunRepository = payrollRunRepository;
        _payslipRepository = payslipRepository;
        _dateTimeProvider = dateTimeProvider;
        _usernameProvider = usernameProvider;
        _options = options.Value;
    }

    public async Task<IEnumerable<PayrollRunModel>> GetRunsAsync() =>
        (await _payrollRunRepository.ListAsync()).Select(ToModel);

    public async Task<PayrollRunModel?> GetRunByIdAsync(int id)
    {
        var run = await _payrollRunRepository.GetByIdAsync(id);
        return run is null ? null : ToModel(run);
    }

    public async Task<PayrollRunModel> CreateRunAsync(CreatePayrollRunModel model)
    {
        var employees = model.DepartmentId is null
            ? (await _employeeRepository.ListAsync()).Where(e => e.Status == EmployeeStatus.Active).ToList()
            : (await _employeeRepository.GetByDepartmentAsync(model.DepartmentId.Value))
                .Where(e => e.Status == EmployeeStatus.Active).ToList();

        var payslips = new List<Payslip>();
        decimal totalGross = 0;
        decimal totalNet = 0;

        foreach (var employee in employees)
        {
            var (regularPay, overtimePay) = await ComputePayAsync(employee, model.PeriodStart, model.PeriodEnd);
            var gross = regularPay + overtimePay;
            var tax = Math.Round(gross * _options.TaxRate, 2);
            var other = Math.Round(_options.DefaultOtherDeductions, 2);
            var net = Math.Round(gross - tax - other, 2);

            totalGross += gross;
            totalNet += net;

            payslips.Add(new Payslip
            {
                EmployeeId = employee.Id,
                RegularPay = Math.Round(regularPay, 2),
                OvertimePay = Math.Round(overtimePay, 2),
                GrossPay = Math.Round(gross, 2),
                TaxDeduction = tax,
                OtherDeductions = other,
                NetPay = net,
                PayDate = model.PayDate
            });
        }

        var run = new PayrollRun
        {
            PeriodStart = model.PeriodStart,
            PeriodEnd = model.PeriodEnd,
            RunDate = _dateTimeProvider.UtcNow,
            CreatedBy = _usernameProvider.GetUsername(),
            Status = PayrollRunStatus.Completed,
            TotalGross = Math.Round(totalGross, 2),
            TotalNet = Math.Round(totalNet, 2)
        };

        run = await _payrollRunRepository.AddAsync(run);
        foreach (var payslip in payslips) payslip.PayrollRunId = run.Id;
        await _payslipRepository.AddRangeAsync(payslips);

        run = await _payrollRunRepository.GetByIdAsync(run.Id);
        return ToModel(run!);
    }

    private async Task<(decimal Regular, decimal Overtime)> ComputePayAsync(
        Employee employee, DateTime periodStart, DateTime periodEnd)
    {
        if (employee.PayType == PayType.Salary)
        {
            var perPeriod = employee.AnnualSalary / _options.PayPeriodsPerYear;
            return (perPeriod, 0m);
        }

        var entries = await _timeEntryRepository.GetByEmployeeAsync(employee.Id, periodStart, periodEnd);
        var regularHours = entries.Sum(e => e.RegularHours);
        var overtimeHours = entries.Sum(e => e.OvertimeHours);
        var regular = regularHours * employee.PayRate;
        var overtime = overtimeHours * employee.PayRate * _options.OvertimeMultiplier;
        return (regular, overtime);
    }

    private static PayrollRunModel ToModel(PayrollRun r) => new()
    {
        Id = r.Id,
        PeriodStart = r.PeriodStart,
        PeriodEnd = r.PeriodEnd,
        RunDate = r.RunDate,
        CreatedBy = r.CreatedBy,
        Status = r.Status,
        TotalGross = r.TotalGross,
        TotalNet = r.TotalNet,
        PayslipCount = r.Payslips?.Count ?? 0
    };
}
