namespace TodoApp.Application.Users.Dtos;

public record JwtDto
{
    /// <summary>
    /// The JWT token.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Token expiry date.
    /// </summary>
    public DateTime? Expiry { get; set; }

    /// <summary>
    /// Indicates if the provided username and password combination is valid.
    /// </summary>
    public bool IsAuthenticatedUser => !string.IsNullOrEmpty(this.Token);
}
