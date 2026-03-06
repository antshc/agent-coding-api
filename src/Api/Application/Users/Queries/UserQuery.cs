using Api.Application.Users.Payload;

namespace Api.Application.Users.Queries;

internal class UserQuery : IUserQuery
{
    public Task<IReadOnlyCollection<GetUserDto>> GetAll(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GetUserDto> GetById(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}