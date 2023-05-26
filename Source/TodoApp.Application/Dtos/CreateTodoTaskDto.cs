namespace TodoApp.Application.Dtos;

public class CreateTodoTaskDto
{
    /// <summary>
    /// A description of the task.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The date the task is due to be completed.
    /// </summary>
    public DateTime? DueDate { get; set; }
}
