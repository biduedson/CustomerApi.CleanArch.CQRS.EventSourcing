

using CustomerApi.Core.SharedKernel;

namespace CustomerApi.Application.Customer.Responses;

public  class CreatedCustomerResponse(Guid Id) : IResponse
{
    public Guid Id { get; } = Id;
}
