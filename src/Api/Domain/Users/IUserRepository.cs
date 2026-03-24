namespace Api.Domain.Users;

public interface IUserRepository
{
    Task<IReadOnlyCollection<User>> GetAll(CancellationToken cancellationToken);

    Task<User?> GetById(int id, CancellationToken cancellationToken);
}
