using System;
using System.Threading.Tasks;
using Api.Application.Users.Payload;
using LightBDD.Framework;
using ApiTests.IntegrationTests.Application.Clients;
using ApiTests.IntegrationTests.Application;
using Features.Common;

namespace Features;

internal class Managing_users_steps(TestWebApplicationFactory app) : Base_api_steps, IDisposable
{
    private readonly IUsersClient _client = app.UsersClient;
    private State<Guid> _orderId;

    public TestWebApplicationFactory App { get; } = app;

    public Task Given_not_exist_user_with_id_USERID(Guid userId)
    {
        _orderId = userId;

        App.IdGeneratorMock.Setup(m => m.New())
            .Returns(userId);

        return Task.CompletedTask;
    }

    public async Task When_user_send_create_new_user_request(string firstName, string lastName)
    {
        var request = new CreateUserDto() { FirstName = firstName, LastName = lastName };
        Response = await _client.CreateUser(request);
    }

    public async Task When_user_get_order_details_by_send_get_order_request()
        => Response = await _client.Get(_orderId);

    public void Dispose()
    {
    }
}
