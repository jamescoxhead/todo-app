using AutoMapper;
using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Application.TodoTasks.Dtos;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Mapping;

public class TodoTaskMappingProfile : Profile
{
    public TodoTaskMappingProfile()
    {
        this.CreateMap<CreateTodoTaskCommand, TodoTask>().ForMember(dest => dest.Id, opt => opt.Ignore())
                                                         .ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => false));

        this.CreateMap<TodoTask, TodoTaskDto>().ReverseMap();
    }
}
