using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finance.Services;
using Finance.Services.Models;

namespace Finance.Services.Web.Controllers;

[Authorize]
[ApiController]
public class PayslipsController(IPayslipService payslipService) : Controller
{
    [HttpGet("api/payslips")]
    public async Task<IActionResult> Get([FromQuery] int? employeeId = null, [FromQuery] int? runId = null)
    {
        try
        {
            if (employeeId is not null) return Ok(await payslipService.GetByEmployeeAsync(employeeId.Value));
            if (runId is not null) return Ok(await payslipService.GetByRunAsync(runId.Value));
            return BadRequest("Specify employeeId or runId.");
        }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
