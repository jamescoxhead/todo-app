using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Exceptions;
using TodoApp.Application.Users.Commands;
using TodoApp.Application.Users.Dtos;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> logger;
    private readonly IValidator<CreateUserCommand> createUserCommandValidator;
    private readonly IMediator mediator;

    public UsersController(IMediator mediator, ILogger<UsersController> logger, IValidator<CreateUserCommand> createUserCommandValidator)
    {
        this.mediator = mediator;
        this.logger = logger;
        this.createUserCommandValidator = createUserCommandValidator;
    }

    [HttpPost]
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
}
