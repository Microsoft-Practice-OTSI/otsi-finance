using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finance.Services;
using Finance.Services.Models;

namespace Finance.Services.Web.Controllers;

[Authorize]
[ApiController]
public class EmployeesController(IEmployeeService employeeService) : Controller
{
    [HttpGet("api/employees")]
    public async Task<IActionResult> GetAll()
    {
        try { return Ok(await employeeService.GetAllAsync()); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpGet("api/employees/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await employeeService.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpGet("api/employees/employeeid/{employeeId}")]
    public async Task<IActionResult> GetByEmployeeId(string employeeId)
    {
        try
        {
            var result = await employeeService.GetByEmployeeIdAsync(employeeId);
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpGet("api/departments/{departmentId:int}/employees")]
    public async Task<IActionResult> GetByDepartment(int departmentId)
    {
        try { return Ok(await employeeService.GetByDepartmentAsync(departmentId)); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpPost("api/employees")]
    [Authorize(Policy = "PayrollAdmin")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeModel model)
    {
        try { return Ok(await employeeService.CreateAsync(model)); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpPut("api/employees/{id:int}")]
    [Authorize(Policy = "PayrollAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeModel model)
    {
        try { model.Id = id; return Ok(await employeeService.UpdateAsync(model)); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpDelete("api/employees/{id:int}")]
    [Authorize(Policy = "PayrollAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        try { await employeeService.DeleteAsync(id); return NoContent(); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
