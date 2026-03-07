using System.Collections.Generic;
using Api;
using Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ApiTests.DataTests;

public class DataMigrationTests : IClassFixture<DataFixture>
{
    private readonly DataFixture _fixture;

    public DataMigrationTests(DataFixture fixture) => _fixture = fixture;

    [Fact]
    public void MigrateDatabase_ShouldApplyMigrationsWithoutError()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = _fixture.ConnectionString
        });

        builder.Services.AddDependencyRegistrations(builder.Configuration);
        builder.Services.AddControllers();

        var app = builder.Build();

        PreRunHook.MigrateDatabase(app);

        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        Assert.True(context.Database.CanConnect());
    }
}

