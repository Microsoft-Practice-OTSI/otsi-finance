using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finance.Services;
using Finance.Services.Models;

namespace Finance.Services.Web.Controllers;

[Authorize]
[ApiController]
public class DepartmentsController(IDepartmentService departmentService) : Controller
{
    [HttpGet("api/departments")]
    public async Task<IActionResult> GetAll()
    {
        try { return Ok(await departmentService.GetAllAsync()); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpGet("api/departments/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await departmentService.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpPost("api/departments")]
    [Authorize(Policy = "PayrollAdmin")]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentModel model)
    {
        try { return Ok(await departmentService.CreateAsync(model)); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpPut("api/departments/{id:int}")]
    [Authorize(Policy = "PayrollAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateDepartmentModel model)
    {
        try { return Ok(await departmentService.UpdateAsync(id, model)); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    [HttpDelete("api/departments/{id:int}")]
    [Authorize(Policy = "PayrollAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        try { await departmentService.DeleteAsync(id); return NoContent(); }
        catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
