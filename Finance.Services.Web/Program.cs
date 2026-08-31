using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Finance.Data.TableStorage;
using Finance.Services.Web.Composition;
using Finance.Services.Web.Health;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddConfigurationServices()
    .AddLoggingServices()
    .AddSecurityServices()
    .AddHealthChecks()
    .AddApplicationServices()
    .AddCorsPolicy("SiteCorsPolicy");

builder.Services
    .AddHttpContextAccessor()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers()
    .AddControllersAsServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var store = scope.ServiceProvider.GetRequiredService<IPayrollTableStore>();
    Log.Information("Azure Table Storage target: {ServiceUri} (configured: {Configured})", store.ServiceUri, store.IsConfigured);
    try
    {
        await store.EnsureTablesExistAsync();
    }
    catch (System.Exception ex)
    {
        Log.Warning(ex, "Azure Table Storage initialization failed. Set the AzureStorage:ConnectionString value in appsettings and restart.");
    }
}

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors("SiteCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapHealthChecks("/healthz");
app.MapHealthChecks("/healthz/details", new HealthCheckOptions { ResponseWriter = HealthCheckResponseWriter.WriteResponse })
    .RequireAuthorization();
app.MapControllers();
app.Run();
