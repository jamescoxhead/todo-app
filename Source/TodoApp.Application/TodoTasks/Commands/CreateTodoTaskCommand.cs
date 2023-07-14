using AutoMapper;
using MediatR;
using TodoApp.Application.Interfaces;
using TodoApp.Application.TodoTasks.Dtos;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.TodoTasks.Commands;

public record CreateTodoTaskCommand : IRequest<TodoTaskDto>
{
    /// <summary>
    /// A description of the task.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The date the task is due to be completed.
    /// </summary>
    public DateTime? DueDate { get; set; }
}

public class CreateTodoTaskCommandHandler : IRequestHandler<CreateTodoTaskCommand, TodoTaskDto>
{
    private readonly ITodoDbContext dbContext;
    private readonly IMapper mapper;

    public CreateTodoTaskCommandHandler(ITodoDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<TodoTaskDto> Handle(CreateTodoTaskCommand request, CancellationToken cancellationToken)
    {
        var todoTask = this.mapper.Map<TodoTask>(request);
        todoTask.IsComplete = false;

        this.dbContext.TodoTasks.Add(todoTask);
        await this.dbContext.SaveChangesAsync(cancellationToken);

        var returnValue = this.mapper.Map<TodoTaskDto>(todoTask);

        return returnValue;
    }
}
