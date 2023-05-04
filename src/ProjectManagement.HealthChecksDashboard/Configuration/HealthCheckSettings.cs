namespace ProjectManagement.HealthChecksDashboard.Configuration;

public class HealthCheckSettings
{
    public HealthCheckClient[] Clients { get; set; } = Array.Empty<HealthCheckClient>();
}

public class HealthCheckClient
{
    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;
}