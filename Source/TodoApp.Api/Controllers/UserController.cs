using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TodoApp.Application.Configuration.Models;
using TodoApp.Application.Exceptions;
using TodoApp.Application.Users.Commands;
using TodoApp.Application.Users.Dtos;
using TodoApp.Infrastructure.Identity.Models;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> logger;
    private readonly IValidator<CreateUserCommand> createUserCommandValidator;
    private readonly IMediator mediator;

    public UserController(IMediator mediator, ILogger<UserController> logger, IValidator<CreateUserCommand> createUserCommandValidator)
    {
        this.mediator = mediator;
        this.logger = logger;
        this.createUserCommandValidator = createUserCommandValidator;
    }

    [HttpPost("Register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(CreateUserCommand createModel)
    {
        var validationResult = await this.createUserCommandValidator.ValidateAsync(createModel);

        if (!validationResult.IsValid)
        {
            var problemDetails = new ValidationProblemDetails(validationResult.ToDictionary());
            return this.BadRequest(problemDetails);
        }

        try
        {
            var responseModel = await this.mediator.Send(createModel);

            return this.Ok(responseModel);
        }
        catch (IdentityException ex)
        {
            this.logger.LogError(ex, "Could not create user. {Errors}", string.Join(", ", ex.Errors.Select(error => error.Description)));
            return this.BadRequest();
        }
    }

    [HttpPost("Login")]
    [ProducesResponseType(typeof(JwtDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(AuthenticateUserCommand loginModel)
    {
        try
        {
            var responseModel = await this.mediator.Send(loginModel);

            if (responseModel.IsAuthenticatedUser)
            {
                return this.Ok(responseModel);
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Could not authenticate user {Username}", loginModel.Username);
            return this.BadRequest();
        }

        return this.Unauthorized();
    }
}
