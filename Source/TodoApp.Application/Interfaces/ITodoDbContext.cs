using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces;

/// <summary>
/// Defines tables and operations which can be carried out on the Todo database.
/// </summary>
public interface ITodoDbContext
{
    DbSet<TodoTask> TodoTasks { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
