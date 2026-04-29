using CustomerApi.Query.Abstractions;
using MongoDB.Driver;

namespace CustomerApi.Query.Data.Context.Repositories;

internal abstract class BaseReadOnlyRepository<TQueryModel, TKey>(IReadDbContext context) : IReadOnlyRepository<TQueryModel, TKey>
    where TQueryModel : IQueryModel<TKey>
    where  TKey : IEquatable<TKey>
{
    protected readonly IMongoCollection<TQueryModel> Collection = context.GetCollection<TQueryModel>();

    public async Task<TQueryModel> GetByIdAsync(TKey id)
    {
        using var asyncCusrsor = await Collection.FindAsync(queryModel => queryModel.Id.Equals(id));
        return await asyncCusrsor.FirstOrDefaultAsync();
    }
}
