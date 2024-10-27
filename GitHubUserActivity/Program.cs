using System.Diagnostics;
using GitHubUserActivity.Services;
using GitHubUserActivity.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubUserActivity;

class Program
{
    public static async Task Main(string[] args)
    {
        using var host = HostBuilderSetup.CreateHost(args);

        while (true)
        {
            Console.WriteLine("Enter a GitHub username (or type 'exit' to quit):");
            var inputUserName = Console.ReadLine();

            if (string.Equals(inputUserName, "exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting the application. Goodbye!");
                break;
            }

            if (string.IsNullOrWhiteSpace(inputUserName))
            {
                Console.WriteLine("Please provide a valid GitHub username.");
                continue;
            }

            await ExecuteUserEventsAsync(host.Services, inputUserName);
        }
    }

    private static async Task ExecuteUserEventsAsync(IServiceProvider services, string username)
    {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;

        try
        {
            var gitHubApiService = scopedServices.GetRequiredService<IGitHubApiService>();
            var time = Stopwatch.StartNew();
            time.Start();
            var events = await gitHubApiService.FetchAndFormatUserEvents(username);
            time.Stop();
            Console.WriteLine($"Took {time.ElapsedMilliseconds}ms to execute {events.Count} events.");
            foreach (var gitHubEvent in events)
            {
                Console.WriteLine(ConsoleFormatter.FormatEvent(gitHubEvent));
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}