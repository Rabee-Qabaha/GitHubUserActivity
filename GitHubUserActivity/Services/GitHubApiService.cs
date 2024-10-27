using System.Diagnostics;
using System.Text.Json;
using GitHubUserActivity.Models;
using GitHubUserActivity.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GitHubUserActivity.Services;

public class GitHubApiService : IGitHubApiService
{
    private readonly IMemoryCache _memoryCache;
    private readonly HttpClient _httpClient;
    private readonly ILogger<GitHubApiService> _logger;
    private readonly ApiSettings _apiSettings;

    public GitHubApiService(HttpClient httpClient, ILogger<GitHubApiService> logger,
        IOptions<ApiSettings> apiSettings, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _memoryCache = memoryCache;
        _apiSettings = apiSettings.Value;
    }

    public async Task<List<GitHubEvent>> FetchAndFormatUserEvents(string userName)
    {
        if (_memoryCache.TryGetValue(userName, out List<GitHubEvent> cachedEvents))
        {
            return cachedEvents;
        }
        
        var url = $"{_apiSettings.GitHubApiBaseUrl}/users/{userName}/events";
        
        try
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubUserActivity");

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new HttpRequestException("User not found. Please check the username and try again.");
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var events = JsonSerializer.Deserialize<List<GitHubEvent>>(content);
            _memoryCache.Set(userName, events!, TimeSpan.FromHours(1));
            
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