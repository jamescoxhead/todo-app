using Microsoft.AspNetCore.Identity;

namespace TodoApp.Application.Exceptions;

/// <summary>
/// Thrown when an identity related error occurs.
/// </summary>
public class IdentityException : Exception
{
    /// <summary>
    /// The errors which caused the exception.
    /// </summary>
    public IEnumerable<IdentityError> Errors { get; private set; }

    /// <summary>
    /// Username of the affected user.
    /// </summary>
    public string Username { get; private set; }

    /// <summary>
    /// Creates a new instance of <see cref="IdentityException"/>.
    /// </summary>
    /// <param name="message">A message describing the exception.</param>
    /// <param name="username">The username of the affected user.</param>
    /// <param name="errors">The errors which caused the exception.</param>
    public IdentityException(string message, string username, IEnumerable<IdentityError> errors)
        : base(message)
    {
        this.Errors = errors;
        this.Username = username;
    }
}
