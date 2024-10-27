using GitHubUserActivity.Services;
using GitHubUserActivity.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GitHubUserActivity;

public class HostBuilderSetup
{
    public static IHost CreateHost(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<ApiSettings>(context.Configuration.GetSection("ApiSettings"));
                services.AddHttpClient<IGitHubApiService, GitHubApiService>();
                services.AddMemoryCache();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .Build();
    }
}