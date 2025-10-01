using LiteBus.Commands.Extensions.MicrosoftDependencyInjection;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using LiteBus.Queries.Extensions.MicrosoftDependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersApi.Application.Handlers;
using OrdersApi.Infrastructure.Mongo;

namespace OrdersApi.CrossCutting.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {

        services.AddSingleton<MongoDbContext>();


        services.AddLiteBus(liteBus =>
        {
            liteBus.AddCommandModule(module =>
            {
                module.RegisterFromAssembly(typeof(OrdersApi.Application.Handlers.CreateOrderHandler).Assembly);
            });
            
            liteBus.AddQueryModule(module =>
            {
                module.RegisterFromAssembly(typeof(OrdersApi.Application.Handlers.GetAllOrdersHandler).Assembly);
            });
        });


        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<GetAllOrdersHandler>();
        services.AddScoped<GetOrderByIdHandler>();

        return services;
    }
}