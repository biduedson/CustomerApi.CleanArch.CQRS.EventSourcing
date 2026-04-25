
using System.Diagnostics;
using CustomerApi.Core.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerApi.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var commandName = request.GetGenericTypeName();

        logger.LogInformation("Handling {CommandName} with content: {@Request}", commandName, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next(cancellationToken);

        timer.Stop();

        var timeToken = timer.Elapsed.TotalSeconds;

        logger.LogInformation("Handled {CommandName} in {TimeToken} seconds", commandName, timeToken);

        return response;
    }   
}
