using Api.Application.Users.Payload;

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

        context.SaveChanges();
    }
}
