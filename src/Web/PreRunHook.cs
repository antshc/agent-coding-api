using Api.Data;

namespace Api;

internal class PreRunHook
{
    public static void MigrateDatabase(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            DatabaseInitializer.Initialize(context);
        }
    }
}