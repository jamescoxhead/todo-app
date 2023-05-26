using TodoApp.Application.Dtos;

namespace TodoApp.Application.Interfaces;

/// <summary>
/// Defines methods for working with todo tasks.
/// </summary>
public interface ITodoTaskService
{
    /// <summary>
    /// Gets all todo tasks.
    /// </summary>
    /// <returns>A collection of all todo tasks that have been saved.</returns>
    Task<IEnumerable<TodoTaskDto>> GetTodoTasks();

    /// <summary>
    /// Gets a todo task matching an ID.
    /// </summary>
    /// <param name="id">The todo task ID.</param>
    /// <returns>A <see cref="TodoTaskDto"/> containing the specified todo task.</returns>
    Task<TodoTaskDto?> GetTodoTaskById(int id);

    /// <summary>
    /// Creates a new todo task.
    /// </summary>
    /// <param name="createTask">DTO containing the new todo task.</param>
    /// <returns>A <see cref="TodoTaskDto"/> containing the created todo task</returns>
    Task<TodoTaskDto> CreateTodoTask(CreateTodoTaskDto createTask);

    /// <summary>
    /// Updates an existing todo task.
    /// </summary>
    /// <param name="updateTask">DTO containing the todo task to update.</param>
    /// <returns>A <see cref="TodoTaskDto"/> containing the updated todo task.</returns>
    Task<TodoTaskDto> UpdateTodoTask(UpdateTodoTaskDto updateTask);

    /// <summary>
    /// Deletes an existing todo task.
    /// </summary>
    /// <param name="id">The ID of the todo task to delete.</param>
    Task DeleteTodoTask(int id);
}
