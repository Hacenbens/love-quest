using System.Net;
using System.Net.Http.Json;
using LoveQuest.Web;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LoveQuest.Tests;

public class ApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Health_endpoint_is_ok()
    {
        var res = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Game_page_is_served()
    {
        var html = await _client.GetStringAsync("/");
        Assert.Contains("LOVE QUEST", html);
    }

    [Fact]
    public async Task Quest_does_not_leak_the_letter()
    {
        var json = await _client.GetStringAsync("/api/quest");
        Assert.DoesNotContain("respawn", json);
        Assert.Contains("totalLines", json);
    }

    [Fact]
    public async Task Letter_endpoint_releases_earned_lines()
    {
        var p = await _client.GetFromJsonAsync<LetterProgress>("/api/letter?hearts=5");
        Assert.NotNull(p);
        Assert.Single(p.Unlocked);
    }
}
