using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ApiTests.IntegrationTests;

// why? The integration test strategy follows the official ASP.NET Core approach for hosting the System Under Test (SUT) within a single process.
// WebApplicationFactory<TEntryPoint> bootstraps the SUT in-process, providing a realistic end-to-end test environment without requiring a running server.
// IClassFixture<WebApplicationFactory<Program>> shares a single factory instance across all tests in the class, improving performance.
// CreateClient() returns an HttpClient that automatically follows redirects and handles cookies.
// More: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests
public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected HttpClient HttpClient { get; }

    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        HttpClient = factory.CreateClient();
        HttpClient.DefaultRequestHeaders.Add("accept", "text/plain");
    }
}

