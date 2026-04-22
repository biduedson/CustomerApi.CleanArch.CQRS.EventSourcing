

using CustomerApi.Core.SharedKernel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CustomerApi.Core.AppSettings;

public sealed class CacheOptions : IAppOptions
{
    static string IAppOptions.ConfigureSectionPath => nameof(CacheOptions);

    public int AbsoluteExpirationInHours { get; private init;}
    public int SlidingExpirationInSeconds { get; private init;}
}
