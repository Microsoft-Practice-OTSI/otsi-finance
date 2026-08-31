using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finance.Services;
using Finance.Services.Models;

namespace Finance.Services.Web.Controllers;

[Authorize]
[ApiController]
public class TimeEntriesController(ITimeEntryService timeEntryService) : Controller
{
    [HttpGet("api/time-entries")]
    public async Task<IActionResult> GetByEmployee([FromQuery] int employeeId, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
    {
        try { return Ok(await timeEntryService.GetByEmployeeAsync(employeeId, from, to)); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpPost("api/time-entries")]
    [Authorize(Policy = "PayrollAdmin")]
    public async Task<IActionResult> Create([FromBody] CreateTimeEntryModel model)
    {
        try { return Ok(await timeEntryService.CreateAsync(model)); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpDelete("api/time-entries/{id:int}")]
    [Authorize(Policy = "PayrollAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        try { await timeEntryService.DeleteAsync(id); return NoContent(); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
