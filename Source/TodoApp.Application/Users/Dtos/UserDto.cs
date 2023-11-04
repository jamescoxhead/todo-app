namespace TodoApp.Application.Users.Dtos;

public record UserDto
{
    /// <summary>
    /// The user's unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The user's username used to sign in.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
