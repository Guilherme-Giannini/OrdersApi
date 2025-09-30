using LiteBus.Queries;
using LiteBus.Queries.Abstractions;
using OrdersApi.Domain.Entities;
using System.Collections.Generic;

namespace OrdersApi.Application.Queries;

public sealed record GetAllOrdersQuery() : IQuery<List<Order>>;