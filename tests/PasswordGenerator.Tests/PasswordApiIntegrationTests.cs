using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PasswordGenerator.Api.Data;

namespace PasswordGenerator.Tests;

public sealed class PasswordApiIntegrationTests : IClassFixture<PasswordApiTestApplicationFactory>
{
    private readonly HttpClient _client;

    public PasswordApiIntegrationTests(PasswordApiTestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostPasswords_ShouldCreatePasswordAndReturnId()
    {
        var response = await _client.PostAsync("/passwords", null);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<PasswordCreatedResponse>();

        Assert.NotNull(payload);
        Assert.NotEqual(Guid.Empty, payload!.Id);
    }

    [Fact]
    public async Task GetPasswordsById_ShouldReturnPassword_WhenRecordExists()
    {
        var createdResponse = await _client.PostAsync("/passwords", null);
        var createdPayload = await createdResponse.Content.ReadFromJsonAsync<PasswordCreatedResponse>();

        Assert.NotNull(createdPayload);

        var response = await _client.GetAsync($"/passwords/{createdPayload!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.NotNull(payload);
        Assert.Equal(createdPayload.Id, payload!.Id);
        Assert.False(string.IsNullOrWhiteSpace(payload.Password));
    }

    [Fact]
    public async Task GetPasswordsById_ShouldReturnBadRequest_WhenGuidIsInvalid()
    {
        var response = await _client.GetAsync("/passwords/not-a-guid");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();

        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails!.Status);
        Assert.Contains("Identificador", problemDetails.Title, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetPasswordsById_ShouldReturnNotFound_WhenGuidDoesNotExist()
    {
        var response = await _client.GetAsync($"/passwords/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();

        Assert.NotNull(problemDetails);
        Assert.Equal(404, problemDetails!.Status);
        Assert.Contains("Senha", problemDetails.Title, StringComparison.OrdinalIgnoreCase);
    }

    private sealed record ProblemDetailsResponse
    {
        public string? Title { get; set; }
        public int? Status { get; set; }
        public string? Detail { get; set; }
        public string? Type { get; set; }
    }
}

public sealed class PasswordApiTestApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<PasswordGeneratorDbContext>>();
            services.RemoveAll<PasswordGeneratorDbContext>();

            _connection.Open();
            services.AddSingleton(_connection);
            services.AddDbContext<PasswordGeneratorDbContext>((_, options) =>
                options.UseSqlite(_connection));

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PasswordGeneratorDbContext>();
            dbContext.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }

        base.Dispose(disposing);
    }
}

public sealed record PasswordCreatedResponse(Guid Id);

public sealed record PasswordResponse(Guid Id, string Password);
