using ProjectManagement.HealthChecksDashboard.Data;
using Steeltoe.Discovery.Client;

namespace ProjectManagement.HealthChecksDashboard.Extensions;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddSingleton<WeatherForecastService>();
        
        services.AddDiscoveryClient(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
        });
    }
}