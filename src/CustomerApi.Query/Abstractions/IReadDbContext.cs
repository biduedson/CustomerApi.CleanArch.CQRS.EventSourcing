using MongoDB.Driver;

namespace CustomerApi.Query.Abstractions;

public  interface IReadDbContext : IDisposable
{
    string ConnectionString { get; }

    IMongoCollection<TQueryModel> GEtCollection<TQueryModel>() where TQueryModel : IQueryModel;

    Task CreateCollectionsAsync();
}
