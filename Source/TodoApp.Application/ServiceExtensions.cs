using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;

namespace TodoApp.Application;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds application layer services to the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with application services.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ITodoTaskService, TodoTaskService>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
