using LiteBus;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OrdersApi.Application;
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
            liteBus.Register(new CreateOrderHandler());
            liteBus.Register(new GetAllOrdersHandler());
            liteBus.Register(new GetOrderByIdHandler());

     
        });

   

        return services;
    }
}
}