using LiteBus.Commands;
using LiteBus.Commands.Abstractions;
namespace OrdersApi.Application.Commands;

public sealed record CreateOrderCommand(string Product, int Quantity) : ICommand<string>;

