using Microsoft.Extensions.Options;
using ProjectManagement.HealthChecksDashboard.Abstractions;
using ProjectManagement.HealthChecksDashboard.Configuration;

namespace ProjectManagement.HealthChecksDashboard.Services;

public class HealthCheckService : IHealthCheckService
{
    private readonly ILogger<HealthCheckService> _logger;
    private readonly HealthCheckOptions _options;

    public HealthCheckService(ILogger<HealthCheckService> logger, IOptionsSnapshot<HealthCheckOptions> settings)
    {
        _logger = logger;
        _options = settings.Value;
    }

    public async Task CheckHealthAsync()
    {
        foreach (HealthCheckClient client in _options.Clients)
        {
            _logger.LogInformation("Checking health for {Name}...", client.Name);
        }
    }
}