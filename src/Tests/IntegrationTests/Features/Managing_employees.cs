using System;
using System.Net;
using System.Threading.Tasks;
using LightBDD.Framework.Expectations;
using LightBDD.XUnit2;
using ApiTests.IntegrationTests.Features.Common;

namespace Features;

public class Managing_employees : Base_feature
{
    [Scenario]
    public async Task Successful_employee_creation()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.Given_no_employees_exist(),
            s => s.When_a_caller_sends_a_create_employee_request("Jane", "Smith", 52000m, new DateOnly(1990, 5, 15)),
            s => s.Then_the_response_has_status_STATUSCODE_and_location_header(Expect.To.Equal(HttpStatusCode.Created)));
    }

    [Scenario]
    public async Task Blank_first_name_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.Given_no_employees_exist(),
            s => s.When_a_caller_sends_a_create_employee_request("", "Smith", 52000m, new DateOnly(1990, 5, 15)),
            s => s.Then_the_response_has_status_and_success(
                Expect.To.Equal(HttpStatusCode.BadRequest),
                Expect.To.Equal(false)));
    }

    [Scenario]
    public async Task Blank_last_name_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.Given_no_employees_exist(),
            s => s.When_a_caller_sends_a_create_employee_request("Jane", "", 52000m, new DateOnly(1990, 5, 15)),
            s => s.Then_the_response_has_status_and_success(
                Expect.To.Equal(HttpStatusCode.BadRequest),
                Expect.To.Equal(false)));
    }

    [Scenario]
    public async Task Missing_salary_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.Given_no_employees_exist(),
            s => s.When_a_caller_sends_a_create_employee_request("Jane", "Smith", null, new DateOnly(1990, 5, 15)),
            s => s.Then_the_response_has_status_and_success(
                Expect.To.Equal(HttpStatusCode.BadRequest),
                Expect.To.Equal(false)));
    }

    [Scenario]
    public async Task Negative_salary_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.Given_no_employees_exist(),
            s => s.When_a_caller_sends_a_create_employee_request("Jane", "Smith", -1m, new DateOnly(1990, 5, 15)),
            s => s.Then_the_response_has_status_and_success(
                Expect.To.Equal(HttpStatusCode.BadRequest),
                Expect.To.Equal(false)));
    }

    [Scenario]
    public async Task Missing_date_of_birth_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.Given_no_employees_exist(),
            s => s.When_a_caller_sends_a_create_employee_request("Jane", "Smith", 52000m, null),
            s => s.Then_the_response_has_status_and_success(
                Expect.To.Equal(HttpStatusCode.BadRequest),
                Expect.To.Equal(false)));
    }

    [Scenario]
    public async Task Future_date_of_birth_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.Given_no_employees_exist(),
            s => s.When_a_caller_sends_a_create_employee_request(
                "Jane", "Smith", 52000m, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))),
            s => s.Then_the_response_has_status_and_success(
                Expect.To.Equal(HttpStatusCode.BadRequest),
                Expect.To.Equal(false)));
    }

    [Scenario]
    public async Task Date_of_birth_older_than_120_years_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.Given_no_employees_exist(),
            s => s.When_a_caller_sends_a_create_employee_request(
                "Jane", "Smith", 52000m, DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-121))),
            s => s.Then_the_response_has_status_and_success(
                Expect.To.Equal(HttpStatusCode.BadRequest),
                Expect.To.Equal(false)));
    }
}
