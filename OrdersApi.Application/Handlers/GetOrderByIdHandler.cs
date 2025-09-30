using LiteBus.Queries;
using LiteBus.Queries.Abstractions;
using MongoDB.Bson;
using MongoDB.Driver;
using OrdersApi.Application.Queries;
using OrdersApi.Domain.Entities;
using OrdersApi.Infrastructure.Mongo;

namespace OrdersApi.Application.Handlers;

public class GetOrderByIdHandler : IQueryHandler<GetOrderByIdQuery, Order?>
{
    private readonly MongoDbContext _context;
    public GetOrderByIdHandler(MongoDbContext context) => _context = context;

    public async Task<Order?> HandleAsync(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.Id, query.Id);
        return await _context.Orders.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}