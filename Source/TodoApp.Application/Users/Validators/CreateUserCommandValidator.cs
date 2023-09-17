using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TodoApp.Application.Users.Commands;

namespace TodoApp.Application.Users.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private const string AtLeastOneUppercaseErrorMessage = "'{PropertyName}' must contain at least one uppercase letter";
    private const string AtLeastOneLowercaseErrorMessage = "'{PropertyName}' must contain at least one lowercase letter";
    private const string AtLeastOneDigitErrorMessage = "'{PropertyName}' must contain at least one number";
    private const string AtLeastOneNonAlphaNumericErrorMessage = "'{PropertyName}' must contain at least one symbol";

    public CreateUserCommandValidator(IOptions<IdentityOptions> identityOptions)
    {
        var passwordOptions = identityOptions.Value.Password;

        this.RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        this.RuleFor(c => c.Password).MinimumLength(passwordOptions.RequiredLength)
            .Matches(@"[A-Z]+").WithMessage(AtLeastOneUppercaseErrorMessage)
            .Matches(@"[a-z]+").WithMessage(AtLeastOneLowercaseErrorMessage)
            .Matches(@"\d+").WithMessage(AtLeastOneDigitErrorMessage)
            .Matches(@"[^a-zA-Z\d\s:]+").WithMessage(AtLeastOneNonAlphaNumericErrorMessage);
    }
}
