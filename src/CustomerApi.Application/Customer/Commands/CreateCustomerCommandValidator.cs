
using FluentValidation;

namespace CustomerApi.Application.Customer.Commands;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(command => command.LastName)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254);
        RuleFor(command => command.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.Today);
    }
}
