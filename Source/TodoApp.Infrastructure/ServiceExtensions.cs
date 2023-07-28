using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Interfaces;
using TodoApp.Infrastructure.Identity.Models;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Infrastructure.Persistence.Seeding;

namespace TodoApp.Infrastructure;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds infrastructure layer services to the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with infrastructure services.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TodoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("TodoAppDbConnection"), builder => builder.MigrationsAssembly(typeof(TodoDbContext).Assembly.FullName)));
        services.AddScoped<ITodoDbContext>(provider => provider.GetRequiredService<TodoDbContext>());

        services.AddScoped<TodoDbInitialiser>();

        services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<TodoDbContext>();

        return services;
    }
}
