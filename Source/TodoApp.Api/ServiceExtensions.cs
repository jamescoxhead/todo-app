using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Api;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds API layer services to the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with API services.</returns>
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHealthChecks()
                .AddDbContextCheck<TodoDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;

            var passwordOptions = configuration.GetSection("Identity:PasswordOptions").Get<PasswordOptions>();
            options.Password = passwordOptions ?? new PasswordOptions();
        });

        return services;
    }
}
