using LiteBus.Commands;
using LiteBus.Commands.Abstractions;
using LiteBus.Queries;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;
using OrdersApi.Application.Commands;
using OrdersApi.Application.Queries;
using OrdersApi.Domain.Entities;
using OrdersApi.Models;

namespace OrdersApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICommandMediator _commandMediator;
    private readonly IQueryMediator _queryMediator;

    public OrdersController(ICommandMediator commandMediator, IQueryMediator queryMediator)
    {
        _commandMediator = commandMediator;
        _queryMediator = queryMediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        var command = new CreateOrderCommand(dto.Product, dto.Quantity);
        var id = await _commandMediator.SendAsync(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _queryMediator.QueryAsync(new GetAllOrdersQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var order = await _queryMediator.QueryAsync(new GetOrderByIdQuery(id));
        if (order is null) return NotFound();
        return Ok(order);
    }
}