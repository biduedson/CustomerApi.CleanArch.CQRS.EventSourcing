

using CustomerApi.Core.SharedKernel;
using Microsoft.Extensions.Configuration;

namespace CustomerApi.Core.Extensions;

public static class ConfigurationExtensions
{
    public static TOptions GEtOptions<TOptions>(this IConfiguration configuration)
        where TOptions : class, IAppOptions
    {
        return configuration
            .GetRequiredSection(TOptions.ConfigureSectionPath)
            .Get<TOptions>(options => options.BindNonPublicProperties = true);
    }
}
