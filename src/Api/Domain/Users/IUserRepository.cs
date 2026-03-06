namespace Api.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetById(int id, CancellationToken cancellationToken);
}