using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Identity.Models;

namespace TodoApp.Infrastructure.Persistence;

/// <summary>
/// Represents a session with the Todo database.
/// </summary>
public class TodoDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, ITodoDbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<TodoTask> TodoTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
