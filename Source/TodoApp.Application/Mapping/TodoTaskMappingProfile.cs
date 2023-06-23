using AutoMapper;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Mapping;

public class TodoTaskMappingProfile : Profile
{
    public TodoTaskMappingProfile()
    {
        this.CreateMap<CreateTodoTaskDto, TodoTask>().ForMember(dest => dest.Id, opt => opt.Ignore())
                                                     .ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => false));

        this.CreateMap<TodoTask, TodoTaskDto>().ReverseMap();
    }
}
