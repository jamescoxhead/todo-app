using FluentValidation;
using TodoApp.Application.Dtos;

namespace TodoApp.Application.Validation;

public class CreateTodoTaskDtoValidator : AbstractValidator<CreateTodoTaskDto>
{
    private const string DateInPastErrorMessage = "'{PropertyName}' cannot be in the past";

    public CreateTodoTaskDtoValidator()
    {
        this.RuleFor(c => c.Description).NotEmpty().MaximumLength(750);
        this.RuleFor(c => c.DueDate).GreaterThan(DateTime.UtcNow.Date).WithMessage(DateInPastErrorMessage);
    }
}
