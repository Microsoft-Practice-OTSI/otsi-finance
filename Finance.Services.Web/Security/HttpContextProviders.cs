using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Finance.Common;

namespace Finance.Services.Web.Security;

public class HttpContextUsernameProvider : IUsernameProvider
{
    private readonly IHttpContextAccessor _accessor;
    private readonly string _claimType;

    public HttpContextUsernameProvider(IHttpContextAccessor accessor, string claimType = "name")
    {
        _accessor = accessor;
        _claimType = claimType;
    }

    public string GetUsername()
    {
        var user = _accessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            var claim = user.FindFirst(_claimType) ?? user.FindFirst(ClaimTypes.NameIdentifier);
            if (claim is not null) return claim.Value;
        }
        return "system";
    }
}

public class HttpContextTokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _accessor;

    public HttpContextTokenProvider(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string GetToken()
    {
        var auth = _accessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (auth is not null && auth.StartsWith("Bearer ", System.StringComparison.OrdinalIgnoreCase))
            return auth.Substring("Bearer ".Length).Trim();
        return string.Empty;
    }
}
