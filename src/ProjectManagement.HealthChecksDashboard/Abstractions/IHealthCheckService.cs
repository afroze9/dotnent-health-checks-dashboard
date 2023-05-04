namespace ProjectManagement.HealthChecksDashboard.Abstractions;

public interface IHealthCheckService
{
    Task CheckHealthAsync();
}