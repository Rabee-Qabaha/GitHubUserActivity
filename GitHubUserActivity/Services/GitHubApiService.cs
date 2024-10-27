using System.Text.Json;
using GitHubUserActivity.Models;
using GitHubUserActivity.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GitHubUserActivity.Services;

public class GitHubApiService : IGitHubApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GitHubApiService> _logger;
    private readonly ApiSettings _apiSettings;

    public GitHubApiService(HttpClient httpClient, ILogger<GitHubApiService> logger, IOptions<ApiSettings> apiSettings)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiSettings = apiSettings.Value;
    }

    public async Task<List<GitHubEvent>> FetchAndFormatUserEvents(string userName)
    {
        try
        {
            string url = $"{_apiSettings.GitHubApiBaseUrl}/users/{userName}/events";
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubUserActivity");
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var events = JsonSerializer.Deserialize<List<GitHubEvent>>(content);
            return events ?? [];
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Request error for user {userName}: {e.Message}");
            throw new HttpRequestException();
        }
        catch (JsonException e)
        {
            _logger.LogError($"JSON parsing error: {e.Message}");
            throw new JsonException();
        }
    }
}