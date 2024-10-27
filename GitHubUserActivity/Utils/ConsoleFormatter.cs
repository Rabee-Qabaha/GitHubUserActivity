using GitHubUserActivity.Models;

namespace GitHubUserActivity.Utils;

public class ConsoleFormatter
{
    public static string FormatEvent(GitHubEvent gitHubEvent)
    {
        if (gitHubEvent == null) return "Invalid event data.";
        
        switch (gitHubEvent.type)
        {
            case "PushEvent":
                int commitCount = gitHubEvent.payload.size ?? 0;
                return $"Pushed {commitCount} commit{(commitCount != 1 ? "s" : "")} to {gitHubEvent.repo.name}";

            case "issuesEvent":
                string action = char.ToUpper(gitHubEvent.payload.action[0]) + gitHubEvent.payload.action.Substring(1);
                return $"{action} an issue in {gitHubEvent.repo.name}";

            case "WatchEvent":
                return $"Starred {gitHubEvent.repo.name}";

            case "issueCommentEvent":
                string commentAction = char.ToUpper(gitHubEvent.payload.action[0]) + gitHubEvent.payload.action.Substring(1);
                var issueTitle = gitHubEvent.payload.issue.title;
                return $"{commentAction} a comment on issue \"{issueTitle}\" in {gitHubEvent.repo.name}";

            case "PullRequestEvent":
                var prAction = char.ToUpper(gitHubEvent.payload.action[0]) + gitHubEvent.payload.action.Substring(1);
                return $"{prAction} a pull request in {gitHubEvent.repo.name}";

            default:
                return $"Performed {gitHubEvent.type} on {gitHubEvent.repo.name}";
        }
    }
}