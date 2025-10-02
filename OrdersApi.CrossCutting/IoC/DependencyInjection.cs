using LiteBus.Commands.Abstractions;
using LiteBus.Commands.Extensions.MicrosoftDependencyInjection;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using LiteBus.Queries.Abstractions;
using LiteBus.Queries.Extensions.MicrosoftDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using OrdersApi.Application.Commands;
using OrdersApi.Application.Handlers;
using OrdersApi.Application.Queries;
using OrdersApi.Domain.Entities;
using OrdersApi.Infrastructure.Mongo;

namespace OrdersApi.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbContext>();

            services.AddScoped<ICommandHandler<CreateOrderCommand, string>, CreateOrderHandler>();
            services.AddScoped<IQueryHandler<GetAllOrdersQuery, List<Order>>, GetAllOrdersHandler>();
            services.AddScoped<IQueryHandler<GetOrderByIdQuery, Order?>, GetOrderByIdHandler>();

            services.AddLiteBus(liteBus =>
            {
                liteBus.AddCommandModule(module =>
                {
                    module.RegisterFromAssembly(typeof(CreateOrderHandler).Assembly);
                });

                liteBus.AddQueryModule(module =>
                {
                    module.RegisterFromAssembly(typeof(GetAllOrdersHandler).Assembly);
                });
            });
            services.Decorate<ICommandHandler<CreateOrderCommand, string>, LoggingCommandDecorator<CreateOrderCommand, string>>();
            services.Decorate<IQueryHandler<GetAllOrdersQuery, List<Order>>, LoggingQueryDecorator<GetAllOrdersQuery, List<Order>>>();
            services.Decorate<IQueryHandler<GetOrderByIdQuery, Order?>, LoggingQueryDecorator<GetOrderByIdQuery, Order?>>();

            return services;
        }
    }

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
            var result = await _inner.HandleAsync(command, cancellationToken);

            // Pós-processamento: Sentry + Console
            SentrySdk.AddBreadcrumb($"Command {typeof(TCommand).Name} executado com sucesso.");
            System.Console.WriteLine($"Command {typeof(TCommand).Name} result: {result}");

            return result;
        }
    }
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

            SentrySdk.AddBreadcrumb($"Query {typeof(TQuery).Name} executada com sucesso.");
            System.Console.WriteLine($"Query {typeof(TQuery).Name} result: {result?.ToString() ?? "null"}");

            return result;
        }
    }
}