using Api.Application.Users.Payload;

namespace Api.Application.Users.Queries;

public interface IUserQuery
{
    Task<IReadOnlyCollection<GetUserDto>> GetAll(CancellationToken cancellationToken);

    Task<GetUserDto> GetById(int id, CancellationToken cancellationToken);
}