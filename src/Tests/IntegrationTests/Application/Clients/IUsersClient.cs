using System;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Application.Users.Payload;
using RestEase;

namespace ApiTests.IntegrationTests.Application.Clients;

[BasePath("users")]
[AllowAnyStatusCode]
public interface IUsersClient
{
    [Post()]
    Task<HttpResponseMessage> CreateUser([Body] CreateUserDto userRequest);

    [Get("{userId}")]
    Task<HttpResponseMessage> Get([Path] Guid userId);
}
