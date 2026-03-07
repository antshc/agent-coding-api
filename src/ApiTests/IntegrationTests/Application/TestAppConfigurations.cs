using System.Collections.Generic;
using ApiTests.IntegrationTests.Application.Infrastructure;

namespace ApiTests.IntegrationTests.Application;

public class FeatureTestAppConfigurations(MsSqlDbContainerMock dbContainerFixtureMock) : TestAppConfigurations(dbContainerFixtureMock)
{
    public override IDictionary<string, string> Get()
    {
        var configs = base.Get();
        // configs.Add("Features:WhatsupProvider", bool.TrueString);

        return configs;
    }
}

public class TestAppConfigurations(MsSqlDbContainerMock dbContainerFixtureMock)
{
    public virtual IDictionary<string, string> Get()
        => new Dictionary<string, string>() { { "ConnectionStrings:DefaultConnection", dbContainerFixtureMock.DbConnectionString } };
}
