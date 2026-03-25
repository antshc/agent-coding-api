using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Application.Employees.Payload;
using LightBDD.Framework;
using LightBDD.Framework.Parameters;
using ApiTests.IntegrationTests.Application;
using ApiTests.IntegrationTests.Application.Clients;
using Features.Common;

namespace Features;

internal sealed class Managing_employees_steps(TestWebApplicationFactory app) : Base_api_steps, IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly IEmployeesClient _client = app.EmployeesClient;

    public Task Given_no_employees_exist() => Task.CompletedTask;

    public async Task When_a_caller_sends_a_create_employee_request(
        string firstName, string lastName, decimal? salary, DateOnly? dateOfBirth)
    {
        var request = new CreateEmployeeDto
        {
            FirstName = firstName,
            LastName = lastName,
            Salary = salary,
            DateOfBirth = dateOfBirth
        };
        Response = await _client.CreateEmployee(request);
    }

    public async Task Then_the_response_has_status_and_success(
        Verifiable<HttpStatusCode> expectedStatus, Verifiable<bool> expectedSuccess)
    {
        expectedStatus.SetActual(Response.GetValue().StatusCode);

        var body = await GetResponseBodyAsync<ApiResponseWrapper<GetEmployeeDto>>();
        expectedSuccess.SetActual(body?.Success ?? false);
    }

    public Task Then_the_response_has_status_STATUSCODE_and_location_header(
        Verifiable<HttpStatusCode> expectedStatus)
    {
        expectedStatus.SetActual(Response.GetValue().StatusCode);
        var location = Response.GetValue().Headers.Location;

        if (location is null)
            throw new InvalidOperationException("Expected a Location header but none was found.");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }

    private async Task<T?> GetResponseBodyAsync<T>()
    {
        var content = await Response.GetValue().Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, JsonOptions);
    }

    private sealed class ApiResponseWrapper<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }
    }
}
