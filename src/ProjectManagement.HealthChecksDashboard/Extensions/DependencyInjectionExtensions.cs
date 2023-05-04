using ProjectManagement.HealthChecksDashboard.Abstractions;
using ProjectManagement.HealthChecksDashboard.Configuration;
using ProjectManagement.HealthChecksDashboard.Data;
using ProjectManagement.HealthChecksDashboard.Services;
using Steeltoe.Discovery.Client;

namespace ProjectManagement.HealthChecksDashboard.Extensions;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    private static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HealthCheckOptions>(configuration.GetSection("HealthCheck"));
        services.AddScoped<IHealthCheckService, HealthCheckService>();
        services.AddHostedService<HealthCheckBackgroundService>();
    }
    
    public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDiscoveryClient(configuration);
        services.AddHealthChecks(configuration);
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddSingleton<WeatherForecastService>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
        });
    }
}