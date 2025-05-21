using System.Diagnostics;
using Application.Common.Messaging;
using Ardalis.Result;
using Domain.Common.Messaging;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;
internal static class LoggingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var timer = new Stopwatch();
            timer.Start();

            Result<TResponse> result = await innerHandler.Handle(command, cancellationToken);

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;

            if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings
            {
                logger.LogWarning(
                    "[PERFORMANCE] The request {Request} took {TimeTaken} seconds. with data = {RequestData}",
                    typeof(TCommand).Name,
                    timeTaken.Seconds,
                    command
                );
            }

            return result;
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandBaseHandler<TCommand>> logger)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var timer = new Stopwatch();
            timer.Start();

            Result result = await innerHandler.Handle(command, cancellationToken);

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;

            if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings
            {
                logger.LogWarning(
                    "[PERFORMANCE] The request {Request} took {TimeTaken} seconds. with data = {RequestData}",
                    typeof(TCommand).Name,
                    timeTaken.Seconds,
                    command
                );
            }

            return result;
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            var timer = new Stopwatch();
            timer.Start();

            Result<TResponse> result = await innerHandler.Handle(query, cancellationToken);

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;

            if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings
            {
                logger.LogWarning(
                    "[PERFORMANCE] The request {Request} took {TimeTaken} seconds. with data = {RequestData}",
                    typeof(TQuery).Name,
                    timeTaken.Seconds,
                    query
                );
            }

            return result;
        }
    }
}
