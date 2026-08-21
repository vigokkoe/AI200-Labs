using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AI200Labs.Shared.Extensions;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Adds shared and project-specific appsettings.json files to the configuration.
    /// Automatically resolves appsettings.shared.json from the solution root.
    /// </summary>
    public static HostApplicationBuilder AddAppSettings(this HostApplicationBuilder builder)
    {
        // Resolve shared settings path relative to the solution root
        var sharedSettingsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "appsettings.shared.json");

        // If not found at solution root, try one level up (when running from project directory)
        if (!File.Exists(sharedSettingsPath))
        {
            sharedSettingsPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "appsettings.shared.json");
        }

        builder.Configuration
            .AddJsonFile(sharedSettingsPath, optional: false)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables();

        return builder;
    }
}
