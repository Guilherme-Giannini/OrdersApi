using LiteBus.Queries.Abstractions;
using OrdersApi.Domain.Entities;

namespace OrdersApi.Application.Queries;

public sealed record GetAllOrdersQuery() : IQuery<List<Order>>;