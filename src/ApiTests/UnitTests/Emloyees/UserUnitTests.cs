using System;
using Api.Domain.Employees;
using Xunit;

namespace ApiTests.UnitTests.Emloyees;

public class EmployeeUnitTests
{
    private static readonly DateOnly ValidDateOfBirth = new(1990, 5, 15);
    private const string ValidFirstName = "Jane";
    private const string ValidLastName = "Smith";
    private const decimal ValidSalary = 52000m;

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_throws_for_blank_first_name(string firstName)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Employee(firstName, ValidLastName, ValidSalary, ValidDateOfBirth));

        Assert.Equal("firstName", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_throws_for_blank_last_name(string lastName)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Employee(ValidFirstName, lastName, ValidSalary, ValidDateOfBirth));

        Assert.Equal("lastName", exception.ParamName);
    }

    [Fact]
    public void Constructor_throws_for_negative_salary()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Employee(ValidFirstName, ValidLastName, -1m, ValidDateOfBirth));

        Assert.Equal("salary", exception.ParamName);
    }

    [Fact]
    public void Constructor_throws_for_future_date_of_birth()
    {
        var futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        var exception = Assert.Throws<ArgumentException>(() =>
            new Employee(ValidFirstName, ValidLastName, ValidSalary, futureDate));

        Assert.Equal("dateOfBirth", exception.ParamName);
    }

    [Fact]
    public void Constructor_throws_for_date_of_birth_more_than_120_years_ago()
    {
        var tooOldDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-121));

        var exception = Assert.Throws<ArgumentException>(() =>
            new Employee(ValidFirstName, ValidLastName, ValidSalary, tooOldDate));

        Assert.Equal("dateOfBirth", exception.ParamName);
    }
}
