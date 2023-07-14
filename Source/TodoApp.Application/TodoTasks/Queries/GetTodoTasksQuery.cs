using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Application.TodoTasks.Dtos;

namespace TodoApp.Application.TodoTasks.Queries;

public record GetTodoTasksQuery : IRequest<IEnumerable<TodoTaskDto>>
{
}

public class GetTodoTasksQueryHandler : IRequestHandler<GetTodoTasksQuery, IEnumerable<TodoTaskDto>>
{
    private readonly ITodoDbContext dbContext;
    private readonly IMapper mapper;

    public GetTodoTasksQueryHandler(ITodoDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<TodoTaskDto>> Handle(GetTodoTasksQuery request, CancellationToken cancellationToken)
    {
        var todoTasks = await this.dbContext.TodoTasks.ToListAsync(cancellationToken);
        var returnValue = todoTasks.Select(task => this.mapper.Map<TodoTaskDto>(task));

        return returnValue;
    }
}
