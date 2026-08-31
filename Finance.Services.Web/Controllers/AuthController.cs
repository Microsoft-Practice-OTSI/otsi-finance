using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finance.Services;
using Finance.Services.Models;

namespace Finance.Services.Web.Controllers;

[ApiController]
public class AuthController(IAuthService authService) : Controller
{
    [AllowAnonymous]
    [HttpPost("api/auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var token = await authService.AuthenticateAsync(model);
        return token is null ? Unauthorized() : Ok(token);
    }
}
