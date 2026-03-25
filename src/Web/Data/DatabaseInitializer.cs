using Api.Domain.Employees;
using Api.Domain.Users;

namespace Api.Data;

public static class DatabaseInitializer
{
    public static void Initialize(DataContext context)
    {
        // Recreate the database each time.

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Seed if there are no employees.
        if (context.Users.Any())
        {
            return;
        }

        context.Users.AddRange(
            new User("LeBron", "James"),
            new User("Ja", "Morant"),
            new User("Megan", "Rapinoe"));

        context.Employees.AddRange(
            new Employee("LeBron", "James", 100000m, new DateOnly(1984, 12, 30)),
            new Employee("Ja", "Morant", 80000m, new DateOnly(1999, 8, 10)),
            new Employee("Megan", "Rapinoe", 70000m, new DateOnly(1985, 7, 5)));

        context.SaveChanges();
    }
}
