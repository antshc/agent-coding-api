using System;

namespace ApiTests.IntegrationTests.Application;

public static class TestEmployees
{
    public static readonly TestEmployee Default = new();
}

public record TestEmployee
{
    public string FirstName { get; init; } = "Jane";
    public string LastName { get; init; } = "Smith";
    public decimal Salary { get; init; } = 52000m;
    public DateOnly DateOfBirth { get; init; } = new DateOnly(1990, 5, 15);
}
