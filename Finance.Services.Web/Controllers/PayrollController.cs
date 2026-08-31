using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finance.Services;
using Finance.Services.Models;

namespace Finance.Services.Web.Controllers;

[Authorize]
[ApiController]
public class PayrollController(IPayrollService payrollService) : Controller
{
    [HttpGet("api/payroll")]
    public async Task<IActionResult> GetRuns()
    {
        try { return Ok(await payrollService.GetRunsAsync()); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpGet("api/payroll/{id:int}")]
    public async Task<IActionResult> GetRun(int id)
    {
        try
        {
            var result = await payrollService.GetRunByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpPost("api/payroll")]
    [Authorize(Policy = "PayrollAdmin")]
    public async Task<IActionResult> CreateRun([FromBody] CreatePayrollRunModel model)
    {
        try { return Ok(await payrollService.CreateRunAsync(model)); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
