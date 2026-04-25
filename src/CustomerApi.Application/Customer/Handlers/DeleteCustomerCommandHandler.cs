

using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using CustomerApi.Application.Customer.Commands;
using CustomerApi.Core.SharedKernel;
using CustomerApi.Domain.Entities.CustomerAggregate;
using FluentValidation;
using MediatR;

namespace CustomerApi.Application.Customer.Handlers;

public class DeleteCustomerCommandHandler(
    IValidator<DeleteCustomerCommand> validator,
    ICustomerWriteOnlyRepository repository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteCustomerCommand, Result>
{
    public async Task<Result> Handle(
        DeleteCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var customer = await  repository.GetByIdAsync(request.Id);

        if(customer is null)
            return Result.NotFound($"Customer with ID {request.Id} not found.");

        customer.Delete();

        repository.Remove(customer);

        await unitOfWork.SaveChangesAsync();

        return Result.SuccessWithMessage($"Customer with ID {request.Id} has been deleted successfully.");
    }
}
