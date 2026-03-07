using Api.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<User?> GetById(int id, CancellationToken cancellationToken)
    {
        return await context.Users
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
