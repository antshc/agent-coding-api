using System.Collections.Generic;
using ApiTests.IntegrationTests.Application.Infrastructure;

namespace ApiTests.IntegrationTests.Application;

public class FeatureTestAppConfigurations : TestAppConfigurations
{
    public FeatureTestAppConfigurations(MsSqlDbContainerMock dbContainerFixtureMock)
        : base(dbContainerFixtureMock)
    {
    }

    public override IDictionary<string, string> Get()
    {
        var configs = base.Get();
        // configs.Add("Features:WhatsupProvider", bool.TrueString);

        return configs;
    }
}

public class TestAppConfigurations
{
    private readonly MsSqlDbContainerMock _dbContainerFixtureMock;

    public TestAppConfigurations(MsSqlDbContainerMock dbContainerFixtureMock) => _dbContainerFixtureMock = dbContainerFixtureMock;

    public virtual IDictionary<string, string> Get()
        => new Dictionary<string, string>() { { "ConnectionStrings:DefaultConnection", _dbContainerFixtureMock.DbConnectionString } };
}
