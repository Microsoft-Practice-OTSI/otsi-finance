using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finance.Data.Repositories;
using Finance.Services.Models;

namespace Finance.Services;

public class PayslipService : IPayslipService
{
    private readonly IPayslipRepository _repository;

    public PayslipService(IPayslipRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PayslipModel>> GetByEmployeeAsync(int employeeId) =>
        (await _repository.GetByEmployeeAsync(employeeId)).Select(ToModel);

    public async Task<IEnumerable<PayslipModel>> GetByRunAsync(int payrollRunId) =>
        (await _repository.GetByRunAsync(payrollRunId)).Select(ToModel);

    private static PayslipModel ToModel(Finance.Data.Entities.Payslip p) => new()
    {
        Id = p.Id,
        EmployeeKey = p.EmployeeId,
        EmployeeId = p.Employee?.EmployeeId ?? string.Empty,
        EmployeeName = p.Employee is null ? string.Empty : $"{p.Employee.FirstName} {p.Employee.LastName}",
        RegularPay = p.RegularPay,
        OvertimePay = p.OvertimePay,
        GrossPay = p.GrossPay,
        TaxDeduction = p.TaxDeduction,
        OtherDeductions = p.OtherDeductions,
        NetPay = p.NetPay,
        PayDate = p.PayDate
    };
}
