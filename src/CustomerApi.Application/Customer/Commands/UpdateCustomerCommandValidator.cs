

using FluentValidation;

namespace CustomerApi.Application.Customer.Commands;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
            RuleFor(command => command.Id)
                .NotEmpty();
            RuleFor(command => command.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(254);
    }
}
