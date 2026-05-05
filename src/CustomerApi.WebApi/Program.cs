using System.Globalization;
using System.IO.Compression;
using Asp.Versioning;
using CorrelationId;
using CorrelationId.DependencyInjection;
using CustomerApi.Application;
using CustomerApi.Core;
using CustomerApi.Core.Extensions;
using CustomerApi.Infrastructure;
using CustomerApi.Query;
using CustomerApi.WebApi.Extensions;
using FluentValidation;
using FluentValidation.Resources;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Scalar.AspNetCore;
using StackExchange.Profiling;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<GzipCompressionProviderOptions>(compressionOptions => compressionOptions.Level = CompressionLevel.Fastest)
    .Configure<JsonOptions>(jsonOption => jsonOption.JsonSerializerOptions.Configure())
    .Configure<RouteOptions>(routeOptions => routeOptions.LowercaseUrls = true)
    .AddHttpClient()
    .AddHttpContextAccessor()
    .AddResponseCompression(compressionOptions =>
    {
        compressionOptions.EnableForHttps = true;
        compressionOptions.Providers.Add<GzipCompressionProvider>();
    })
    .AddEndpointsApiExplorer()
    .AddApiVersioning(versioningOptions =>
    {
        versioningOptions.DefaultApiVersion = ApiVersion.Default;
        versioningOptions.ReportApiVersions = true;
        versioningOptions.AssumeDefaultVersionWhenUnspecified = true;
    })
    .AddApiExplorer(explorerOptions =>
    {
        explorerOptions.GroupNameFormat = "'v'VVV";
        explorerOptions.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddOpenApi();
builder.Services.AddDataProtection();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(behaviorOptions =>
    {
        behaviorOptions.SuppressMapClientErrors = true;
        behaviorOptions.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(_ => { });


builder.Services
    .ConfigureAppSettings()
    .AddInfrastructure()
    .AddCommandHandlers()
    .AddQueryHandlers()
    .AddWriteDbContext(builder.Environment)
    .AddMongoDbConfiguration()
    .AddWriteOnlyRepositories()
    .AddReadDbContext()
    .AddReadOnlyRepositories()
    .AddCacheService(builder.Configuration)
    .AddHealtChecks(builder.Configuration)
    .AddDefaultCorrelationId();


builder.Services.AddMiniProfiler(options =>
{

    options.RouteBasePath = "/profiler";
    options.ColorScheme = ColorScheme.Dark;
    options.EnableServerTimingHeader = true;
    options.TrackConnectionOpenClose = true;
    options.EnableDebugMode = builder.Environment.IsDevelopment();
}).AddEntityFramework();

// Validação dos serviços adicionados no ASP.NET Core DI.
builder.Host.UseDefaultServiceProvider((context, serviceProviderOptions) =>
{
    serviceProviderOptions.ValidateScopes = context.HostingEnvironment.IsDevelopment();
    serviceProviderOptions.ValidateOnBuild = true;
});

// Configuração do servidor Kestrel para ambientes Linux.
builder.WebHost.UseKestrel(kestrelOptions => kestrelOptions.AddServerHeader = false);

// Configuração global do FluentValidation.
ValidatorOptions.Global.DisplayNameResolver = (_, member, _) => member?.Name;
ValidatorOptions.Global.LanguageManager = new LanguageManager { Enabled = true, Culture = new CultureInfo("en-US") };

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configuração de Health Checks para monitoramento de serviços.
app.UseHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Configuração do OpenAPI para documentação da API.
app.MapOpenApi();

// Rota para referência do Scalar API: /scalar/v1
app.MapScalarApiReference(scalarOptions =>
{
    scalarOptions.DarkMode = true;
    scalarOptions.DotNetFlag = false;
    scalarOptions.DocumentDownloadType = DocumentDownloadType.None;
    scalarOptions.HideModels = true;
    scalarOptions.Title = "Customer.API";
});


// Aplicação de Middlewares no pipeline da API.
app.UseErrorHandling();
app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseMiniProfiler();
app.UseCorrelationId();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAppAsync();