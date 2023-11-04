using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Application.Configuration.Models;
using TodoApp.Application.Users.Dtos;
using TodoApp.Infrastructure.Identity.Models;

namespace TodoApp.Application.Users.Commands;

public record AuthenticateUserCommand : IRequest<JwtDto>
{
    /// <summary>
    /// The user's username
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The user's password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, JwtDto>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly JwtOptions jwtOptions;

    public AuthenticateUserCommandHandler(UserManager<ApplicationUser> userManager, IOptions<JwtOptions> jwtOptions)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
    }

    public async Task<JwtDto> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await this.userManager.FindByNameAsync(request.Username);
        var returnValue = new JwtDto();

        if (existingUser != null && await this.userManager.CheckPasswordAsync(existingUser, request.Password))
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, request.Username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtOptions.Secret));
            var tokenExpiry = DateTime.Now.AddHours(this.jwtOptions.ExpiresHours);

            var token = new JwtSecurityToken(this.jwtOptions.Issuer, this.jwtOptions.Audience, claims, null, tokenExpiry, new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            returnValue.Token = new JwtSecurityTokenHandler().WriteToken(token);
            returnValue.Expiry = token.ValidTo;
        }

        return returnValue;
    }
}
