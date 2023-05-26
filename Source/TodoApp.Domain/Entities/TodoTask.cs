namespace TodoApp.Domain.Entities;

/// <summary>
/// Database entity describing an individual task.
/// </summary>
public class TodoTask : BaseEntity
{
    /// <summary>
    /// A description of the task.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The date the task is due to be completed.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Indicates if the task has been completed.
    /// </summary>
    public bool IsComplete { get; set; }
}
