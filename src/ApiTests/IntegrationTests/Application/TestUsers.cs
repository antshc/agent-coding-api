using System;

namespace ApiTests.IntegrationTests.Application;

public static class TestUsers
{
    public static readonly TestUser JohnDoe = new();
}

public record TestUser
{
    public Guid Id { get; init; } = new Guid("DD3B5A02-F730-4762-8BBD-6AB86418B04D");

    public string FirstName { get; init; } = "John";

    public string LastName { get; init; } = "Doe";
}
