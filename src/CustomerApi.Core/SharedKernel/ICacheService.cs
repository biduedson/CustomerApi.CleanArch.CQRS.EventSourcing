

namespace CustomerApi.Core.SharedKernel;

public interface ICacheService
{
    Task<TItem> GetOrCreateAsync<TItem>(string chacheKey, Func<TItem> factory);
    Task<IReadOnlyList<TItem>> GetOrCreateAsync<TItem>(string chacheKey, Func<Task<IReadOnlyList<TItem>>> factory);
    Task RemoveAsync(params string[] cacheKey);
}
