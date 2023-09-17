using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TodoApp.Application.Exceptions;
using TodoApp.Application.Users.Dtos;
using TodoApp.Infrastructure.Identity.Models;

namespace TodoApp.Application.Users.Commands;

public record CreateUserCommand : IRequest<UserDto>
{
    /// <summary>
    /// The user's username used to sign in.
    /// </summary>
    public string UserName => this.Email;

    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The user's password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IMapper mapper;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var applicationUser = this.mapper.Map<ApplicationUser>(request);
        var createResult = await this.userManager.CreateAsync(applicationUser, request.Password);

        if (!createResult.Succeeded)
        {
            throw new IdentityException("Could not create user", request.Email, createResult.Errors);
        }

        var returnValue = this.mapper.Map<UserDto>(applicationUser);

        return returnValue;
    }
}
