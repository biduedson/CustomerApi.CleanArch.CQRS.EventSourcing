
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using CustomerApi.Application.Customer.Commands;
using CustomerApi.Core.SharedKernel;
using CustomerApi.Domain.Entities.CustomerAggregate;
using CustomerApi.Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace CustomerApi.Application.Customer.Handlers;

public class UpdateCustomerCommandHandler(
    IValidator<UpdateCustomerCommand> validator,
    ICustomerWriteOnlyRepository repository,
        IUnitOfWork unitOfWork
    ) :  IRequestHandler<UpdateCustomerCommand, Result>
{
    public async Task<Result> Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if(!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var customer = await repository.GetByIdAsync(request.Id);

        if (customer is null)
            return Result.NotFound($"Customer with ID {request.Id} not found.");

        var newEmail =  Email.Create(request.Email!);

        var emailExisting = await repository.ExistsByEmailAsync(newEmail, request.Id);

        if(emailExisting)
            return Result.Conflict($"Email {request.Email} is already in use.");

        customer.ChangeEmail(newEmail);

        repository.Update(customer);

        await unitOfWork.SaveChangesAsync();

        return Result.SuccessWithMessage("Customer updated successfully.");
    }    
}
