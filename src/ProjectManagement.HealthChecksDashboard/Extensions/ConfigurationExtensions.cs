using ProjectManagement.HealthChecksDashboard.Configuration;
using Winton.Extensions.Configuration.Consul;

namespace ProjectManagement.HealthChecksDashboard.Extensions;

[ExcludeFromCodeCoverage]
public static class ConfigurationExtensions
{
    public static void AddApplicationConfiguration(this ConfigurationManager configuration)
    {
        configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables();

        // Settings for consul kv
        ConsulKVSettings consulKvSettings = new ();
        configuration.GetRequiredSection("ConsulKV").Bind(consulKvSettings);
        configuration.AddConsulKv(consulKvSettings);

        HealthCheckSettings healthCheckSettings = new ();
        configuration.GetRequiredSection("HealthCheck").Bind(healthCheckSettings);

        foreach (HealthCheckClient client in healthCheckSettings.Clients)
        {
            Console.WriteLine($"{client.Name} : {client.Url}");
        }
    }
    
    private static void AddConsulKv(this IConfigurationBuilder builder, ConsulKVSettings settings)
    {
        builder.AddConsul(settings.Key, options =>
        {
            options.ConsulConfigurationOptions = config =>
            {
                config.Address = new Uri(settings.Url);
                config.Token = settings.Token;
            };

            options.Optional = false;
            options.ReloadOnChange = true;
        });
    }
}