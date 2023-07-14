namespace TodoApp.Application.Exceptions;

/// <summary>
/// The exception that is thrown when an entity cannot be found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// The name of the entity.
    /// </summary>
    public string EntityName { get; private set; }

    /// <summary>
    /// The ID that could not be found.
    /// </summary>
    public object Key { get; private set; }

    /// <summary>
    /// Creates a new instance of <see cref="NotFoundException"/>.
    /// </summary>
    /// <param name="entityName">The name of the entity.</param>
    /// <param name="key">The ID that could not be found.</param>
    public NotFoundException(string entityName, object key) : base($"Entity {entityName} with key {key} was not found")
    {
        this.EntityName = entityName;
        this.Key = key;
    }
}
