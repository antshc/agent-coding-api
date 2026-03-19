using Api.Data.Configurations;
using Api.Domain.Employees;
using Api.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

// why? EF Core aligns well with DDD and Onion Architecture by supporting a code-first approach,
// change tracking, LINQ-based querying, and automated schema migrations.
// More: STR21, STR22, STR23
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
