using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Finance.Common;
using Finance.Data;
using Finance.Data.Repositories;
using Finance.Data.TableStorage;
using Finance.Services;
using Finance.Services.Models;
using Finance.Services.Web.Health;
using Finance.Services.Web.Security;

namespace Finance.Services.Web.Composition;

public static class WebApplicationBuilderExtensions
{
    public static void AddCorsPolicy(this WebApplicationBuilder builder, string policyName)
    {
        var corsBuilder = new CorsPolicyBuilder()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials();
        builder.Services.AddCors(options => { options.AddPolicy(policyName, corsBuilder.Build()); });
    }

    public static WebApplicationBuilder AddConfigurationServices(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
            builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);
        return builder;
    }

    public static WebApplicationBuilder AddLoggingServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, lc) =>
        {
            lc.MinimumLevel.Information().Enrich.FromLogContext().WriteTo.Console();
        });
        return builder;
    }

    public static WebApplicationBuilder AddSecurityServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITokenProvider, HttpContextTokenProvider>();
        builder.Services.AddScoped<IUsernameProvider>(s =>
            new HttpContextUsernameProvider(s.GetService<IHttpContextAccessor>()!, "name"));

        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>()
                  ?? throw new InvalidOperationException("Missing required configuration: Jwt");
        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey))
                };
            });

        builder.Services.AddAuthorization(o =>
        {
            o.AddPolicy("PayrollAdmin", p => p.RequireRole("admin", "payroll:admin"));
            o.AddPolicy("PayrollViewer", p => p.RequireRole("admin", "payroll:admin", "payroll:viewer"));
        });

        return builder;
    }

    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IDateTimeProvider, UtcDateTimeProvider>();

        builder.Services.Configure<AzureStorageOptions>(builder.Configuration.GetSection("AzureStorage"));
        builder.Services.AddSingleton<IPayrollTableStore, PayrollTableStore>();

        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
        builder.Services.AddScoped<IPayrollRunRepository, PayrollRunRepository>();
        builder.Services.AddScoped<IPayslipRepository, PayslipRepository>();

        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();
        builder.Services.AddScoped<IPayrollService, PayrollService>();
        builder.Services.AddScoped<IPayslipService, PayslipService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.Configure<PayrollOptions>(builder.Configuration.GetSection("Payroll"));

        return builder;
    }

    public static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddConfigurationExistsHealthCheck(builder.Configuration, TableConstants.ConnectionStringSection)
            .AddConfigurationExistsHealthCheck(builder.Configuration, "Jwt:SigningKey")
            .AddCheck<AzureStorageHealthCheck>("AzureStorage");
        return builder;
    }
}
