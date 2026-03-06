using Api.Application.Users.Queries;
using Api.Data;
using Api.Domain.Users;
using Api.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyRegistrations(this IServiceCollection services, IConfiguration configuration)
    {
        ApplicationLayer(services);
        DomainLayer(services);
        DataLayer(services, configuration);
        SharedKernel(services);

        return services;
    }

    private static void SharedKernel(IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }

    private static void DataLayer(IServiceCollection services, IConfiguration configuration)
    {
        // why? Store Connection strings and credentials securely (e.g., secrets, config providers).
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void ApplicationLayer(IServiceCollection services)
    {
        services.AddScoped<IUserQuery, UserQuery>();
    }

    private static void DomainLayer(IServiceCollection services)
    {
    }
}