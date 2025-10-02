using LiteBus.Commands.Abstractions;
using OrdersApi.Application.Commands;
using OrdersApi.Domain.Entities;
using OrdersApi.Infrastructure.Mongo;

namespace OrdersApi.Application.Handlers;

public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, string>
{
    private readonly MongoDbContext _context;
    public CreateOrderHandler(MongoDbContext context) => _context = context;

    public async Task<string> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order { Product = command.Product, Quantity = command.Quantity };
        await _context.Orders.InsertOneAsync(order, cancellationToken: cancellationToken);
        return order.Id;
    }
}