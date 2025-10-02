using LiteBus.Commands.Abstractions;

public class LoggingCommandDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _inner;

    public LoggingCommandDecorator(ICommandHandler<TCommand, TResult> inner)
    {
        _inner = inner;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        // Executa o handler original
        var result = await _inner.HandleAsync(command, cancellationToken);

        // Pós-processamento
        Console.WriteLine($"Command {typeof(TCommand).Name} executado com resultado: {result}");
        SentrySdk.AddBreadcrumb($"Command {typeof(TCommand).Name} executado com sucesso.");

        return result;
    }
}