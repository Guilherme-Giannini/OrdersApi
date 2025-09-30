using LiteBus.Queries;
using LiteBus.Queries.Abstractions;
using MongoDB.Driver;
using OrdersApi.Application.Queries;
using OrdersApi.Domain.Entities;
using OrdersApi.Infrastructure.Mongo;

namespace OrdersApi.Application.Handlers;

public class GetAllOrdersHandler : IQueryHandler<GetAllOrdersQuery, List<Order>>
{
    private readonly MongoDbContext _context;
    public GetAllOrdersHandler(MongoDbContext context) => _context = context;

    public async Task<List<Order>> HandleAsync(GetAllOrdersQuery query, CancellationToken cancellationToken)
    {
        return await _context.Orders.Find(_ => true).ToListAsync(cancellationToken);
    }
}





