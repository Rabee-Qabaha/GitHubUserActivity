using GitHubUserActivity.Models;

namespace GitHubUserActivity.Services;

public interface IGitHubApiService
{
     Task<List<GitHubEvent>> FetchAndFormatUserEvents(string userName);
}