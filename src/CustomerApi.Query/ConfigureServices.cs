using System.Reflection;
using CustomerApi.Query.Abstractions;
using CustomerApi.Query.Data.Context;
using CustomerApi.Query.Data.Mappings;
using CustomerApi.Query.Data.Repositories.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace CustomerApi.Query;

public static  class ConfigureServices
{

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        var assembly = typeof(IQueryMarker).Assembly;

        return services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly))
            .AddAutoMapper(cfg => { }, assembly)
            .AddValidatorsFromAssembly(assembly);
    }

    public static IServiceCollection AddReadDbContext(this IServiceCollection services)
    {
        services
            .AddScoped<ISynchronizeDb, NoSqlDbContext>()
            .AddScoped<IReadDbContext, NoSqlDbContext>()
            .AddScoped<NoSqlDbContext>();

        return services;
    }

    public static IServiceCollection AddReadOnlyRepositories(this IServiceCollection services) =>
        services.AddScoped<ICustomerReadOnlyRepository, CustomerReadOnlyRepository>();


    public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services, ILogger logger)
    {
        try
        {
            BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

            ConventionRegistry.Register("Conventions",
                new ConventionPack
                {
                    new CamelCaseElementNameConvention(),
                    new EnumRepresentationConvention(BsonType.String),
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfNullConvention(true)
                }, _ => true);

            new CustomerMap().Configure();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao configurar MongoDB");
            throw; 
        }

        return services;
    }
}
