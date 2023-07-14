using AutoMapper;
using MediatR;
using TodoApp.Application.Exceptions;
using TodoApp.Application.Interfaces;
using TodoApp.Application.TodoTasks.Dtos;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.TodoTasks.Commands;

public record UpdateTodoTaskCommand : IRequest<TodoTaskDto>
{
    /// <summary>
    /// The task's unique identifier
    /// </summary>
    public int TodoTaskId { get; set; }

    /// <summary>
    /// Indicates if the task has been completed.
    /// </summary>
    public bool IsComplete { get; set; }
}

public class UpdateTodoTaskCommandHandler : IRequestHandler<UpdateTodoTaskCommand, TodoTaskDto>
{
    private readonly ITodoDbContext dbContext;
    private readonly IMapper mapper;

    public UpdateTodoTaskCommandHandler(ITodoDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<TodoTaskDto> Handle(UpdateTodoTaskCommand request, CancellationToken cancellationToken)
    {
        var existingEntity = await this.dbContext.TodoTasks.FindAsync(new object[] { request.TodoTaskId }, cancellationToken) ??
            throw new NotFoundException(nameof(TodoTask), request.TodoTaskId);
        existingEntity.IsComplete = request.IsComplete;

        await this.dbContext.SaveChangesAsync(cancellationToken);

        var returnValue = this.mapper.Map<TodoTaskDto>(existingEntity);

        return returnValue;
    }
}
