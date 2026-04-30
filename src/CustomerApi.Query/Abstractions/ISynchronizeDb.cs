using System.Linq.Expressions;

namespace CustomerApi.Query.Abstractions;

public interface ISynchronizeDb : IDisposable
{
    Task UpsertAsync<TQueryModel>(TQueryModel queryModel, Expression<Func<TQueryModel, bool>> upserFilter)
        where TQueryModel : IQueryModel;

    Task DeleteAsync<TQueryModel>(Expression<Func<TQueryModel, bool>> deleteFilter)
        where TQueryModel : IQueryModel;
}
