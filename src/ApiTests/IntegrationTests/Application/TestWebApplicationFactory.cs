using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using ApiTests.IntegrationTests.Application.Clients;
using ApiTests.IntegrationTests.LightBDD;
using BenefitsApi.Client.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using RestEase;
using Serilog;

namespace ApiTests.IntegrationTests.Application;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string EnvironmentName = "ComponentTests";
    private readonly TestAppConfigurations _testAppConfigurationsProvider;
    public Mock<IIdGenerator> IdGeneratorMock { get; } = new();

    private readonly DelegatingHandler[] _requestHandlers =
    {
        new HttpRequestLogAsCommentDelegatingHandler(new LightBDDTestLogger<HttpRequestLogAsCommentDelegatingHandler>()), new HttpRequestToCurlDelegatingHandler()
    };

    public TestWebApplicationFactory(
        TestAppConfigurations testAppConfigurationsProvider
    )
    {
        _testAppConfigurationsProvider = testAppConfigurationsProvider;
        HttpClient httpClient = CreateClientWithLogger();
        UsersClient = RestClient.For<IUsersClient>(httpClient);
        UsersClientV2 = new UsersApi(httpClient);
    }

    public IUsersClient UsersClient { get; }
    public UsersApi UsersClientV2 { get; }

    public HttpClient CreateClientWithLogger()
    {
        try
        {
            return CreateDefaultClient(_requestHandlers);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, nameof(CreateClientWithLogger));

            throw;
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseSerilog();

        builder.UseEnvironment(EnvironmentName);
        builder.UseContentRoot(Directory.GetCurrentDirectory());

        builder.ConfigureAppConfiguration(app =>
        {
            var appConfigurationOverrides = _testAppConfigurationsProvider.Get();

            if (appConfigurationOverrides.Any())
            {
                app.AddInMemoryCollection(appConfigurationOverrides);
            }
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IIdGenerator>();
            services.AddSingleton<IIdGenerator>(_ => IdGeneratorMock.Object);
        });
    }
}
