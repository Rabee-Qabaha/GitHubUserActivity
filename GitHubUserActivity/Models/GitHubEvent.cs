namespace GitHubUserActivity.Models;

public class GitHubEvent
{
    public string type { get; set; }
    public GitHubEventPayload payload { get; set; }
    public GitHubEventRepo repo { get; set; }
}