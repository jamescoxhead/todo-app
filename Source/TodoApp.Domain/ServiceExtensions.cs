using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.Domain;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds domain layer services to the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with domain services.</returns>
    public static IServiceCollection AddDomainServices(this IServiceCollection services) => services;
}
