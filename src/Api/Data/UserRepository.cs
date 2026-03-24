using Api.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<IReadOnlyCollection<User>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .OrderBy(user => user.LastName)
            .ThenBy(user => user.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetById(int id, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
