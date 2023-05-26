using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Api;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds API layer services to the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with API services.</returns>
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHealthChecks()
                .AddDbContextCheck<TodoDbContext>();

        return services;
    }
}
