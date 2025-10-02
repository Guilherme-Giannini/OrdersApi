using LiteBus.Queries.Abstractions;
using Sentry;

public class LoggingQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _inner;

    public LoggingQueryDecorator(IQueryHandler<TQuery, TResult> inner)
    {
        _inner = inner;
    }

    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        var result = await _inner.HandleAsync(query, cancellationToken);

        Console.WriteLine($"Query {typeof(TQuery).Name} retornou {result?.ToString() ?? "null"}");
        SentrySdk.AddBreadcrumb($"Query {typeof(TQuery).Name} executada com sucesso.");

        return result;
    }
}