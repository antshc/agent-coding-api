using System.Net.Http;
using System.Threading.Tasks;
using Api.Application.Employees.Payload;
using RestEase;

namespace ApiTests.IntegrationTests.Application.Clients;

[BasePath("api/v1/employees")]
[AllowAnyStatusCode]
public interface IEmployeesClient
{
    [Post]
    Task<HttpResponseMessage> CreateEmployee([Body] CreateEmployeeDto request);
}
