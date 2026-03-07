using Api.Application.Users.Payload;

namespace Api.Data;

public static class DatabaseInitializer
{
    // private static List<GetUserDto> SeedData() => throw new NotImplementedException();

    public static void Initialize(DataContext context)
    {
        // Recreate the database each time.
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Seed if there are no employees.
    }
}
