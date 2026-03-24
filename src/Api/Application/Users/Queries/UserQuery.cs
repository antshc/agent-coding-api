using Api.Application.Users.Payload;
using Api.Domain.Users;

namespace Api.Application.Users.Queries;

internal class UserQuery(IUserRepository userRepository) : IUserQuery
{
    public async Task<IReadOnlyCollection<GetUserDto>> GetAll(CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAll(cancellationToken);

        return users.Select(Map).ToArray();
    }

    public async Task<GetUserDto> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(id, cancellationToken);

        return user is null
            ? throw new ApplicationException("User not found")
            : Map(user);
    }

    private static GetUserDto Map(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName
    };
}
