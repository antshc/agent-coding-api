using System;
using System.Net;
using System.Threading.Tasks;
using LightBDD.XUnit2;
using ApiTests.IntegrationTests.Application;
using ApiTests.IntegrationTests.Features.Common;

namespace Features;

public class Managing_users : Base_feature
{
    [Scenario]
    public async Task Creating_user()
    {
        var orderId = Guid.NewGuid();

        await RunScenarioAsync<Managing_users_steps>(
            s => s.Given_not_exist_user_with_id_USERID(orderId),
            s => s.When_user_send_create_new_user_request(TestUsers.JohnDoe.FirstName, TestUsers.JohnDoe.LastName),
            s => s.Then_response_should_have_status(HttpStatusCode.Created));
    }
}
