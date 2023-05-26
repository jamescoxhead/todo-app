using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Dtos;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services;

/// <summary>
/// Implements <see cref="ITodoTaskService"/>
/// </summary>
public class TodoTaskService : ITodoTaskService
{
    private readonly ITodoDbContext dbContext;

    public TodoTaskService(ITodoDbContext dataContext) => this.dbContext = dataContext;

    public async Task<TodoTaskDto> CreateTodoTask(CreateTodoTaskDto createTask)
    {
        var todoTask = new TodoTask
        {
            Description = createTask.Description,
            DueDate = createTask.DueDate,
            IsComplete = false
        };

        this.dbContext.TodoTasks.Add(todoTask);
        await this.dbContext.SaveChangesAsync();

        var returnValue = MapTodoTask(todoTask);

        return returnValue;
    }

    public async Task DeleteTodoTask(int id)
    {
        var existingEntity = await this.dbContext.TodoTasks.FindAsync(id) ?? throw new Exception($"Could not find todo task with ID {id}");

        this.dbContext.TodoTasks.Remove(existingEntity);
        await this.dbContext.SaveChangesAsync();
    }

    public async Task<TodoTaskDto?> GetTodoTaskById(int id)
    {
        var todoTask = await this.dbContext.TodoTasks.FindAsync(id);

        if (todoTask != null)
        {
            var returnValue = MapTodoTask(todoTask);

            return returnValue;
        }

        return null;
    }

    public async Task<IEnumerable<TodoTaskDto>> GetTodoTasks()
    {
        var todoTasks = await this.dbContext.TodoTasks.ToListAsync();
        var returnValue = todoTasks.Select(task => MapTodoTask(task));

        return returnValue;
    }

    public async Task<TodoTaskDto> UpdateTodoTask(UpdateTodoTaskDto updateTask)
    {
        var existingEntity = await this.dbContext.TodoTasks.FindAsync(updateTask.Id) ?? throw new Exception($"Could not find todo task with ID {updateTask.Id}");
        existingEntity.IsComplete = updateTask.IsComplete;

        await this.dbContext.SaveChangesAsync();

        var returnValue = MapTodoTask(existingEntity);

        return returnValue;
    }

    /// <summary>
    /// Maps a <see cref="TodoTask"/> database entity to a <see cref="TodoTaskDto"/> DTO.
    /// </summary>
    /// <param name="input">The source object.</param>
    /// <returns>A mapped <see cref="TodoTaskDto"/> object.</returns>
    private static TodoTaskDto MapTodoTask(TodoTask input)
    {
        var returnValue = new TodoTaskDto
        {
            Description = input.Description,
            DueDate = input.DueDate,
            Id = input.Id,
            IsComplete = input.IsComplete
        };

        return returnValue;
    }
}
