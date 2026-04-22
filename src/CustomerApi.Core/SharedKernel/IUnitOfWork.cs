
namespace CustomerApi.Core.SharedKernel;

public interface IUnitOfWork : IDisposable
{
    Task SaveChangesAsync();
}
