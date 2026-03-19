using System.Net;
using System.Threading.Tasks;
using LightBDD.XUnit2;
using ApiTests.IntegrationTests.Features.Common;

namespace Features;

public class Managing_employees : Base_feature
{
    [Scenario]
    public async Task Creating_employee_with_valid_data()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.When_creating_employee_with_valid_data(),
            s => s.Then_response_should_have_status(HttpStatusCode.Created));
    }

    [Scenario]
    public async Task Creating_employee_with_blank_first_name_is_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.When_creating_employee_with_blank_first_name(),
            s => s.Then_response_should_have_status(HttpStatusCode.BadRequest));
    }

    [Scenario]
    public async Task Creating_employee_with_blank_last_name_is_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.When_creating_employee_with_blank_last_name(),
            s => s.Then_response_should_have_status(HttpStatusCode.BadRequest));
    }

    [Scenario]
    public async Task Creating_employee_without_salary_is_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.When_creating_employee_without_salary(),
            s => s.Then_response_should_have_status(HttpStatusCode.BadRequest));
    }

    [Scenario]
    public async Task Creating_employee_with_negative_salary_is_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.When_creating_employee_with_negative_salary(),
            s => s.Then_response_should_have_status(HttpStatusCode.BadRequest));
    }

    [Scenario]
    public async Task Creating_employee_without_date_of_birth_is_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.When_creating_employee_without_date_of_birth(),
            s => s.Then_response_should_have_status(HttpStatusCode.BadRequest));
    }

    [Scenario]
    public async Task Creating_employee_with_future_date_of_birth_is_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.When_creating_employee_with_future_date_of_birth(),
            s => s.Then_response_should_have_status(HttpStatusCode.BadRequest));
    }

    [Scenario]
    public async Task Creating_employee_with_date_of_birth_older_than_120_years_is_rejected()
    {
        await RunScenarioAsync<Managing_employees_steps>(
            s => s.When_creating_employee_with_date_of_birth_older_than_120_years(),
            s => s.Then_response_should_have_status(HttpStatusCode.BadRequest));
    }
}
