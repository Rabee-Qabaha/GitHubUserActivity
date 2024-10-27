using GitHubUserActivity.Services;
using GitHubUserActivity.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubUserActivity;

class Program
{
    public static async Task Main(string[] args)
    {
        using var host = HostBuilderSetup.CreateHost(args);

        Console.WriteLine("Enter a GitHub username:");
        var inputUserName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(inputUserName))
        {
            Console.WriteLine("Please provide a valid GitHub username.");
            return;
        }

        await ExecuteUserEventsAsync(host.Services, inputUserName);
    }

    private static async Task ExecuteUserEventsAsync(IServiceProvider services, string username)
    {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;

        try
        {
            var gitHubApiService = scopedServices.GetRequiredService<IGitHubApiService>();
            var events = await gitHubApiService.FetchAndFormatUserEvents(username);

            foreach (var gitHubEvent in events)
            {
                Console.WriteLine(ConsoleFormatter.FormatEvent(gitHubEvent));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}