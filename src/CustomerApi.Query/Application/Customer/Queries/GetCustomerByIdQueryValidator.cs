using FluentValidation;
using MediatR;

namespace CustomerApi.Query.Application.Customer.Queries;

public class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    public GetCustomerByIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .WithMessage("O ID do cliente não pode estar vazio.");
    }
}
