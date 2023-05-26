namespace TodoApp.Domain.Entities;

/// <summary>
/// Base class for database entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// The entity's unique identifier.
    /// </summary>
    public int Id { get; set; }
}
