namespace TodoApp.Application.Dtos;

public class UpdateTodoTaskDto
{
    /// <summary>
    /// The task's unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Indicates if the task has been completed.
    /// </summary>
    public bool IsComplete { get; set; }
}
