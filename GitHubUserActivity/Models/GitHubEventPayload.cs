namespace GitHubUserActivity.Models;

public class GitHubEventPayload
{
    public int? size { get; set; }
    public string action { get; set; }
    public GitHubIssue issue { get; set; }
}