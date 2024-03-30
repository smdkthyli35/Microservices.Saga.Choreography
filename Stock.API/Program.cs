using MassTransit;
using MongoDB.Driver;
using Shared;
using Stock.API.Consumers;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.AddConsumer<PaymentFailedEventConsumer>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_PaymentFailedEventQueue, e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
    });
});

builder.Services.AddSingleton<MongoDbService>();

var app = builder.Build();

using IServiceScope serviceScope = app.Services.CreateScope();
MongoDbService? mongoDbService = serviceScope.ServiceProvider.GetService<MongoDbService>();
IMongoCollection<Stock.API.Models.Stock> stockCollection = mongoDbService.GetCollection<Stock.API.Models.Stock>();
if (!stockCollection.FindSync(s => true).Any())
{
    await stockCollection.InsertOneAsync(new() { ProductId = Guid.NewGuid().ToString(), Count = 100 });
    await stockCollection.InsertOneAsync(new() { ProductId = Guid.NewGuid().ToString(), Count = 150 });
    await stockCollection.InsertOneAsync(new() { ProductId = Guid.NewGuid().ToString(), Count = 200 });
    await stockCollection.InsertOneAsync(new() { ProductId = Guid.NewGuid().ToString(), Count = 50 });
    await stockCollection.InsertOneAsync(new() { ProductId = Guid.NewGuid().ToString(), Count = 5 });
}


app.Run();