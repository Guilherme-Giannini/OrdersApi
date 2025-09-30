using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OrdersApi.Domain.Entities;

namespace OrdersApi.Infrastructure.Mongo;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var conn = configuration["MongoSettings:ConnectionString"] ?? "mongodb://localhost:27017";
        var dbName = configuration["MongoSettings:DatabaseName"] ?? "OrdersDb";
        var client = new MongoClient(conn);
        _database = client.GetDatabase(dbName);
    }

    public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
}

