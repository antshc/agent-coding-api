using Api.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context) => _context = context;

    public async Task<User?> GetById(int id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
