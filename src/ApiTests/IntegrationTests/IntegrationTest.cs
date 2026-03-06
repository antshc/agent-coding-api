using System;
using System.Net.Http;

namespace ApiTests.IntegrationTests;

// why? The integration test strategy follows the official ASP.NET Core approach for hosting the System Under Test (SUT) within a single process.  
// This enables realistic end-to-end testing and debugging. External dependencies such as the database are isolated using Testcontainers, ensuring a clean and reproducible environment for each test run.  
// Scenario-based tests are written using a BDD-style framework to enhance readability. Each scenario can be linked to test case IDs in a test management tool, supporting traceability and visibility during release cycles.
// More: STR50
public class IntegrationTest : IDisposable
{
    private HttpClient? _httpClient;

    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == default)
            {
                _httpClient = new HttpClient
                {
                    //task: update your port if necessary
                    BaseAddress = new Uri("https://localhost:7124")
                };
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            }

            return _httpClient;
        }
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}

