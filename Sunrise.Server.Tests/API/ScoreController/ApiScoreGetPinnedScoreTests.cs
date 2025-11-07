using System.Net;
using Microsoft.AspNetCore.Mvc;
using Sunrise.API.Objects.Keys;
using Sunrise.Tests.Abstracts;
using Sunrise.Tests.Extensions;
using Sunrise.Tests.Utils;
using Sunrise.Tests;

namespace Sunrise.Server.Tests.API.ScoreController;

[Collection("Integration tests collection")]
public class ApiScoreGetPinnedScoreTests(IntegrationDatabaseFixture fixture) : ApiTest(fixture)
{
    [Fact]
    public async Task TestGetPinnedScore()
    {
        // Arrange
        var client = App.CreateClient().UseClient("api").UseUserAuthToken(await GetUserAuthTokens());

        var score = await CreateTestScore();
        score.IsPinned = true;

        // Act
        var response = await client.GetAsync($"score/{score.Id}/pin");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/octet-stream", response.Content.Headers.ContentType?.MediaType);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task TestGetNotExistingScorePin(object id)
    {
        // Arrange
        var client = App.CreateClient().UseClient("api").UseUserAuthToken(await GetUserAuthTokens());

        // Act
        var response = await client.GetAsync($"score/{id}/pin");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseString = await response.Content.ReadFromJsonAsyncWithAppConfig<ProblemDetails>();

        Assert.Equal(ApiErrorResponse.Title.ValidationError, responseString?.Title);
    }

    [Fact]
    public async Task TestGetInvalidScorePin()
    {
        // Arrange
        var client = App.CreateClient().UseClient("api").UseUserAuthToken(await GetUserAuthTokens());

        // Act
        var response = await client.GetAsync("score/invalid/pin");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task TestSetScorePinUnauthorized()
    {
        // Arrange
        var client = App.CreateClient().UseClient("api");

        var score = await CreateTestScore();

        // Act
        var response = await client.PostAsync($"score/{score.Id}/pin", null);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task TestGetPinnedScoreOfRestrictedPlayer()
    {
        // Arrange
        var client = App.CreateClient().UseClient("api").UseUserAuthToken(await GetUserAuthTokens());

        var user = await CreateTestUser();
        var score = await CreateTestScore(user);

        await Database.Users.Moderation.RestrictPlayer(user.Id, null, "Test");

        // Act
        var response = await client.GetAsync($"score/{score.Id}/pin");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}