using System;
using System.Threading.Tasks;
using Api.Application.Employees.Payload;
using LightBDD.Framework;
using ApiTests.IntegrationTests.Application.Clients;
using ApiTests.IntegrationTests.Application;
using Features.Common;

namespace Features;

internal class Managing_employees_steps(TestWebApplicationFactory app) : Base_api_steps, IDisposable
{
    private readonly IEmployeesClient _client = app.EmployeesClient;

    public async Task When_creating_employee_with_valid_data()
    {
        var request = new CreateEmployeeDto
        {
            FirstName = TestEmployees.Default.FirstName,
            LastName = TestEmployees.Default.LastName,
            Salary = TestEmployees.Default.Salary,
            DateOfBirth = TestEmployees.Default.DateOfBirth
        };
        Response = await _client.CreateEmployee(request);
    }

    public async Task When_creating_employee_with_blank_first_name()
    {
        var request = new CreateEmployeeDto
        {
            FirstName = "",
            LastName = TestEmployees.Default.LastName,
            Salary = TestEmployees.Default.Salary,
            DateOfBirth = TestEmployees.Default.DateOfBirth
        };
        Response = await _client.CreateEmployee(request);
    }

    public async Task When_creating_employee_with_blank_last_name()
    {
        var request = new CreateEmployeeDto
        {
            FirstName = TestEmployees.Default.FirstName,
            LastName = "",
            Salary = TestEmployees.Default.Salary,
            DateOfBirth = TestEmployees.Default.DateOfBirth
        };
        Response = await _client.CreateEmployee(request);
    }

    public async Task When_creating_employee_without_salary()
    {
        var request = new CreateEmployeeDto
        {
            FirstName = TestEmployees.Default.FirstName,
            LastName = TestEmployees.Default.LastName,
            Salary = null,
            DateOfBirth = TestEmployees.Default.DateOfBirth
        };
        Response = await _client.CreateEmployee(request);
    }

    public async Task When_creating_employee_with_negative_salary()
    {
        var request = new CreateEmployeeDto
        {
            FirstName = TestEmployees.Default.FirstName,
            LastName = TestEmployees.Default.LastName,
            Salary = -1m,
            DateOfBirth = TestEmployees.Default.DateOfBirth
        };
        Response = await _client.CreateEmployee(request);
    }

    public async Task When_creating_employee_without_date_of_birth()
    {
        var request = new CreateEmployeeDto
        {
            FirstName = TestEmployees.Default.FirstName,
            LastName = TestEmployees.Default.LastName,
            Salary = TestEmployees.Default.Salary,
            DateOfBirth = null
        };
        Response = await _client.CreateEmployee(request);
    }

    public async Task When_creating_employee_with_future_date_of_birth()
    {
        var request = new CreateEmployeeDto
        {
            FirstName = TestEmployees.Default.FirstName,
            LastName = TestEmployees.Default.LastName,
            Salary = TestEmployees.Default.Salary,
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        };
        Response = await _client.CreateEmployee(request);
    }

    public async Task When_creating_employee_with_date_of_birth_older_than_120_years()
    {
        var request = new CreateEmployeeDto
        {
            FirstName = TestEmployees.Default.FirstName,
            LastName = TestEmployees.Default.LastName,
            Salary = TestEmployees.Default.Salary,
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-121))
        };
        Response = await _client.CreateEmployee(request);
    }

    public void Dispose() { }
}
