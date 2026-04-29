using CustomerApi.Query.Abstractions;
using CustomerApi.Query.QueriesModel;

namespace CustomerApi.Query.Data.Context.Repositories.Abstractions;

public interface ICustomerReadOnlyRepository : IReadOnlyRepository<CustomerQueryModel, Guid>
{
    Task<IEnumerable<CustomerQueryModel>> GetAllAsync();
}
