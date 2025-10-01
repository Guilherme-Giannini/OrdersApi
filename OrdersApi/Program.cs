using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using OrdersApi.Application.Commands;
using OrdersApi.Application.Handlers;
using OrdersApi.Application.Queries;
using OrdersApi.CrossCutting.IoC;
using OrdersApi.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencyInjection();

builder.Services.AddTransient<ICommandHandler<CreateOrderCommand, string>, CreateOrderHandler>();
builder.Services.AddTransient<IQueryHandler<GetAllOrdersQuery, List<Order>>, GetAllOrdersHandler>();
builder.Services.AddTransient<IQueryHandler<GetOrderByIdQuery, Order?>, GetOrderByIdHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Orders API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();