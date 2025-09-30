using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OrdersApi.Infrastructure.Mongo;
using OrdersApi.Application.Handlers;

namespace OrdersApi.CrossCutting.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<MongoDbContext>();


        services.AddLiteBus(liteBus =>
        {
        });


        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<GetAllOrdersHandler>();
        services.AddScoped<GetOrderByIdHandler>();

        return services;
    }
}