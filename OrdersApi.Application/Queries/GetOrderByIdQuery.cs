using LiteBus.Queries;
using LiteBus.Queries.Abstractions;
using OrdersApi.Domain.Entities;

namespace OrdersApi.Application.Queries;

public sealed record GetOrderByIdQuery(string Id) : IQuery<Order?>;