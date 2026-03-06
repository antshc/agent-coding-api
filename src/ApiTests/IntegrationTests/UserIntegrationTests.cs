using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Application.Users.Payload;
using Xunit;

namespace ApiTests.IntegrationTests;

public class UserIntegrationTests : IntegrationTest
{
    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees");
        var employees = new List<GetUserDto>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
            },
        };
        await response.ShouldReturn(HttpStatusCode.OK, employees);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/1");
        var employee = new GetUserDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James"
        };
        await response.ShouldReturn(HttpStatusCode.OK, employee);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }
}