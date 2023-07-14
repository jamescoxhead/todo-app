using AutoMapper;
using MediatR;
using TodoApp.Application.Interfaces;
using TodoApp.Application.TodoTasks.Dtos;

namespace TodoApp.Application.TodoTasks.Queries;

public record GetTodoTaskQuery : IRequest<TodoTaskDto?>
{
    /// <summary>
    /// The ID of the task to look up.
    /// </summary>
    public int TodoTaskId { get; set; }
}

public class GetTodoTaskQueryHandler : IRequestHandler<GetTodoTaskQuery, TodoTaskDto?>
{
    private readonly ITodoDbContext dbContext;
    private readonly IMapper mapper;

    public GetTodoTaskQueryHandler(ITodoDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<TodoTaskDto?> Handle(GetTodoTaskQuery request, CancellationToken cancellationToken)
    {
        var todoTask = await this.dbContext.TodoTasks.FindAsync(new object[] { request.TodoTaskId }, cancellationToken);

        if (todoTask != null)
        {
            var returnValue = this.mapper.Map<TodoTaskDto>(todoTask);

            return returnValue;
        }

        return null;
    }
}
