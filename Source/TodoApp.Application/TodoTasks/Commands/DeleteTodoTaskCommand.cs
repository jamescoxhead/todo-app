using MediatR;
using TodoApp.Application.Exceptions;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.TodoTasks.Commands;

public record DeleteTodoTaskCommand : IRequest
{
    public int TodoTaskId { get; set; }
}

public class DeleteTodoTaskCommandHandler : IRequestHandler<DeleteTodoTaskCommand>
{
    private readonly ITodoDbContext dbContext;

    public DeleteTodoTaskCommandHandler(ITodoDbContext dbContext) => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task Handle(DeleteTodoTaskCommand request, CancellationToken cancellationToken)
    {
        var existingEntity = await this.dbContext.TodoTasks.FindAsync(new object[] { request.TodoTaskId }, cancellationToken) ??
            throw new NotFoundException(nameof(TodoTask), request.TodoTaskId);

        this.dbContext.TodoTasks.Remove(existingEntity);
        await this.dbContext.SaveChangesAsync(cancellationToken);
    }
}
