using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence.Seeding;

/// <summary>
/// Seeds test data in the Todo database.
/// </summary>
public class TodoDbInitialiser
{
    private readonly TodoDbContext dbContext;

    /// <summary>
    /// Creates a new instance of <see cref="TodoDbInitialiser"/>.
    /// </summary>
    /// <param name="dbContext">The Entity Framework DB context.</param>
    public TodoDbInitialiser(TodoDbContext dbContext) => this.dbContext = dbContext;

    public async Task SeedDatabaseAsync()
    {
        if (!this.dbContext.TodoTasks.Any())
        {
            var todoTasks = new List<TodoTask>
            {
                new TodoTask { Description = "Build up a task list" },
                new TodoTask { Description = "Tick some items off" },
                new TodoTask { Description = "Make a brew"}
            };

            this.dbContext.TodoTasks.AddRange(todoTasks);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
