using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using OrdersApi.Application.Commands;
using OrdersApi.Application.Handlers;
using OrdersApi.Application.Queries;
using OrdersApi.CrossCutting.IoC;
using OrdersApi.Domain.Entities;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(options =>
{
    options.Dsn = builder.Configuration["Sentry:Dsn"];
    options.TracesSampleRate = 1.0;
});

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OrdersApi"))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter();
});

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