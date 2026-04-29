using CustomerApi.Query.Abstractions;
using CustomerApi.Query.QueriesModel;
using MongoDB.Driver;

namespace CustomerApi.Query.Data.Context.Repositories.Abstractions;

internal class CustomerReadOnlyRepository(IReadDbContext readDbContext)
     : BaseReadOnlyRepository<CustomerQueryModel, Guid>(readDbContext), ICustomerReadOnlyRepository
{
    public async Task<IEnumerable<CustomerQueryModel>> GetAllAsync()
    {
        var sort = Builders<CustomerQueryModel>.Sort
            .Ascending(customer => customer.FirstName)
            .Descending(customer => customer.DateOfBirth);

        var findOptions = new FindOptions<CustomerQueryModel>
        {
            Sort = sort
        };

        using var asyncCursor = await Collection.FindAsync(Builders<CustomerQueryModel>.Filter.Empty, findOptions);
        return await asyncCursor.ToListAsync();
    }

}
