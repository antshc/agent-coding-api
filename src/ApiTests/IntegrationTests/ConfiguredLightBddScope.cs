using System.IO;
using Features;
using LightBDD.Core.Configuration;
using LightBDD.Core.Dependencies;
using LightBDD.Framework.Configuration;
using LightBDD.Framework.Notification;
using LightBDD.XUnit2;
using ApiTests.IntegrationTests;
using ApiTests.IntegrationTests.Application;
using ApiTests.IntegrationTests.Application.Infrastructure;
using ApiTests.IntegrationTests.LightBDD;
using Serilog;

[assembly: ClassCollectionBehavior(AllowTestParallelization = true)]
[assembly: ConfiguredLightBddScope]

namespace ApiTests.IntegrationTests;

internal class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
{
    protected override void OnConfigure(LightBddConfiguration configuration)
    {
        configuration.DependencyContainerConfiguration()
            .UseDefault(ConfigureDI);

        configuration.ExecutionExtensionsConfiguration()
            .RegisterGlobalSetUp<MsSqlDbContainerMock>();

        SetLogging(configuration);
    }

    private static void SetLogging(LightBddConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                Path.Combine(Directory.GetCurrentDirectory(), "tests-logs.txt"),
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}{NewLine}")
            .CreateLogger();

        var currentFeatureNotifier = configuration.FeatureProgressNotifierConfiguration().Notifier;
        var currentScenarioNotifierProvider = configuration.ScenarioProgressNotifierConfiguration().NotifierProvider;
        var serilogProgressNotifier = new SerilogLightBDDProgressNotifier();

        configuration.FeatureProgressNotifierConfiguration()
            .UpdateNotifier(new DelegatingFeatureProgressNotifier(currentFeatureNotifier, serilogProgressNotifier));

        configuration.ScenarioProgressNotifierConfiguration()
            .UpdateNotifierProvider<object>(fixture => new DelegatingScenarioProgressNotifier(currentScenarioNotifierProvider(fixture), serilogProgressNotifier));
    }

    private void ConfigureDI(IDefaultContainerConfigurator cfg)
    {
        cfg.RegisterType<MsSqlDbContainerMock>(InstanceScope.Single);
        cfg.RegisterType<Managing_users_steps>(InstanceScope.Scenario);
        cfg.RegisterType<TestWebApplicationFactory>(InstanceScope.Scenario);
        cfg.RegisterType<TestAppConfigurations>(InstanceScope.Scenario);
        cfg.RegisterType<FeatureTestAppConfigurations>(InstanceScope.Scenario);
    }
}
