using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Finance.Common;
using Finance.Services.Models;

namespace Finance.Services;

public class AuthService : IAuthService
{
    private readonly JwtConfiguration _jwt;
    private readonly IConfiguration _configuration;

    public AuthService(IOptions<JwtConfiguration> jwt, IConfiguration configuration)
    {
        _jwt = jwt.Value;
        _configuration = configuration;
    }

    public Task<TokenModel?> AuthenticateAsync(LoginModel model)
    {
        var adminSection = _configuration.GetSection("AdminUser");
        var adminUser = adminSection["Username"] ?? "admin";
        var adminPassword = adminSection["Password"] ?? "admin123";

        if (!string.Equals(model.Username, adminUser, StringComparison.OrdinalIgnoreCase)
            || model.Password != adminPassword)
        {
            return Task.FromResult<TokenModel?>(null);
        }

        var roles = new List<string> { "admin", "payroll:admin" };
        var token = BuildToken(adminUser, roles);
        return Task.FromResult<TokenModel?>(token);
    }

    private TokenModel BuildToken(string username, IList<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> { new(ClaimTypes.Name, username) };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes);
        var token = new JwtSecurityToken(
            _jwt.Issuer,
            _jwt.Audience,
            claims,
            expires: expires,
            signingCredentials: creds);

        return new TokenModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expires,
            Username = username,
            Roles = roles
        };
    }
}
