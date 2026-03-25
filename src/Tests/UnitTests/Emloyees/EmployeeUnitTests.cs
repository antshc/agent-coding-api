using System;
using Api.Domain.Employees;
using Xunit;

namespace ApiTests.UnitTests.Emloyees;

public class EmployeeUnitTests
{
    private static readonly DateOnly ValidDateOfBirth = new(1990, 5, 15);

    [Fact]
    public void Constructor_throws_when_first_name_is_blank()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Employee("", "Smith", 50000m, ValidDateOfBirth));

        Assert.Contains("First name", ex.Message);
    }

    [Fact]
    public void Constructor_throws_when_last_name_is_blank()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Employee("Jane", "", 50000m, ValidDateOfBirth));

        Assert.Contains("Last name", ex.Message);
    }

    [Fact]
    public void Constructor_throws_when_salary_is_negative()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Employee("Jane", "Smith", -1m, ValidDateOfBirth));

        Assert.Contains("Salary", ex.Message);
    }

    [Fact]
    public void Constructor_throws_when_date_of_birth_is_in_the_future()
    {
        var futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        var ex = Assert.Throws<ArgumentException>(() =>
            new Employee("Jane", "Smith", 50000m, futureDate));

        Assert.Contains("future", ex.Message);
    }

    [Fact]
    public void Constructor_throws_when_date_of_birth_is_more_than_120_years_ago()
    {
        var tooOldDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-121));

        var ex = Assert.Throws<ArgumentException>(() =>
            new Employee("Jane", "Smith", 50000m, tooOldDate));

        Assert.Contains("120 years", ex.Message);
    }
}
