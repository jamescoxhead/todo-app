using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Exceptions;
using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Application.TodoTasks.Dtos;
using TodoApp.Application.TodoTasks.Queries;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoTasksController : ControllerBase
{
    private readonly ILogger<TodoTasksController> logger;
    private readonly IValidator<CreateTodoTaskCommand> createTodoTaskValidator;
    private readonly IMediator mediator;

    public TodoTasksController(ILogger<TodoTasksController> logger, IValidator<CreateTodoTaskCommand> createTodoTaskValidator, IMediator mediator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.createTodoTaskValidator = createTodoTaskValidator ?? throw new ArgumentNullException(nameof(createTodoTaskValidator));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(typeof(TodoTaskDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTodoTasks()
    {
        var model = await this.mediator.Send(new GetTodoTasksQuery());

        return this.Ok(model);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TodoTaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTodoTask(int id)
    {
        var model = await this.mediator.Send(new GetTodoTaskQuery { TodoTaskId = id });

        if (model == null)
        {
            return this.NotFound();
        }

        return this.Ok(model);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TodoTaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTodoTask(CreateTodoTaskCommand createModel)
    {
        var validationResult = await this.createTodoTaskValidator.ValidateAsync(createModel);

        if (!validationResult.IsValid)
        {
            var problemDetails = new ValidationProblemDetails(validationResult.ToDictionary());
            return this.BadRequest(problemDetails);
        }

        var responseModel = await this.mediator.Send(createModel);

        return this.Ok(responseModel);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TodoTaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTodoTask(int id, UpdateTodoTaskCommand updateModel)
    {
        if (id != updateModel.TodoTaskId)
        {
            this.ModelState.AddModelError(nameof(id), "id does not match ID provided in body");
            return this.BadRequest(this.ModelState);
        }

        try
        {
            var responseModel = await this.mediator.Send(updateModel);

            return this.Ok(responseModel);
        }
        catch (NotFoundException ex)
        {
            this.logger.LogError(ex, "Could not find Todo Task with ID {Id}", id);
            return this.NotFound();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Could not update Todo Task with ID {Id}", id);
            return this.BadRequest();
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTodoTask(int id)
    {
        try
        {
            await this.mediator.Send(new DeleteTodoTaskCommand { TodoTaskId = id });
        }
        catch (NotFoundException ex)
        {
            this.logger.LogError(ex, "Could not find Todo Task with ID {Id}", id);
            return this.NotFound();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Could not update Todo Task with ID {Id}", id);
            return this.BadRequest();
        }

        return this.NoContent();
    }
}
