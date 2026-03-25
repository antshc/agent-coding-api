using System.Threading.Tasks;
using Testcontainers.MsSql;
using Xunit;

namespace ApiTests.DataTests;

public class DataFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder().Build();

    public string ConnectionString => _sqlContainer.GetConnectionString();

    public Task InitializeAsync() => _sqlContainer.StartAsync();

    public Task DisposeAsync() => _sqlContainer.StopAsync();
}
