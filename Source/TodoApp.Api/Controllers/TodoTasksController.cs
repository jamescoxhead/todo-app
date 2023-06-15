using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dtos;
using TodoApp.Application.Exceptions;
using TodoApp.Application.Interfaces;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoTasksController : ControllerBase
{
    private readonly ITodoTaskService todoTaskService;
    private readonly ILogger<TodoTasksController> logger;

    public TodoTasksController(ITodoTaskService todoTaskService, ILogger<TodoTasksController> logger)
    {
        this.todoTaskService = todoTaskService ?? throw new ArgumentNullException(nameof(todoTaskService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(typeof(TodoTaskDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTodoTasks()
    {
        var model = await this.todoTaskService.GetTodoTasks();

        return this.Ok(model);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TodoTaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTodoTask(int id)
    {
        var model = await this.todoTaskService.GetTodoTaskById(id);

        if (model == null)
        {
            return this.NotFound();
        }

        return this.Ok(model);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TodoTaskDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTodoTask(CreateTodoTaskDto createModel)
    {
        var responseModel = await this.todoTaskService.CreateTodoTask(createModel);

        return this.Ok(responseModel);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TodoTaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTodoTask(int id, UpdateTodoTaskDto updateModel)
    {
        if (id != updateModel.Id)
        {
            this.ModelState.AddModelError(nameof(id), "id does not match ID provided in body");
            return this.BadRequest(this.ModelState);
        }

        try
        {
            var responseModel = await this.todoTaskService.UpdateTodoTask(updateModel);

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
            await this.todoTaskService.DeleteTodoTask(id);
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
