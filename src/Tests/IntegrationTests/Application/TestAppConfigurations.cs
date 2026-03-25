using System.Collections.Generic;
using ApiTests.IntegrationTests.Application.Infrastructure;

namespace ApiTests.IntegrationTests.Application;

public class TestAppConfigurations(MsSqlDbContainerMock dbContainerFixtureMock)
{
    public virtual IDictionary<string, string> Get()
        => new Dictionary<string, string>() { { "ConnectionStrings:DefaultConnection", dbContainerFixtureMock.DbConnectionString } };
}
