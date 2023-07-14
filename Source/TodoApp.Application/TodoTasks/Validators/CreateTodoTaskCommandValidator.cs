using FluentValidation;
using TodoApp.Application.TodoTasks.Commands;

namespace TodoApp.Application.TodoTasks.Validators;

public class CreateTodoTaskCommandValidator : AbstractValidator<CreateTodoTaskCommand>
{
    private const string DateInPastErrorMessage = "'{PropertyName}' cannot be in the past";

    public CreateTodoTaskCommandValidator()
    {
        this.RuleFor(c => c.Description).NotEmpty().MaximumLength(750);
        this.RuleFor(c => c.DueDate).GreaterThan(DateTime.UtcNow.Date).WithMessage(DateInPastErrorMessage);
    }
}
