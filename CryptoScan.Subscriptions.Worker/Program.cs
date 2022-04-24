using CryptoScan.Constants;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new ConnectionFactory
{
    HostName = builder.Configuration["RabbitMQ:HostName"], 
    Port = Convert.ToInt32(builder.Configuration["RabbitMQ:Port"]), 
    UserName = builder.Configuration["RabbitMQ:UserName"], 
    Password = builder.Configuration["RabbitMQ:Password"]
});

builder.Services.AddHostedService<SubscriptionChangeMessageReceiver>();

var app = builder.Build();
app.Run();

public class SubscriptionChangeMessageReceiver : BackgroundService
{
    private ConnectionFactory _connectionFactory;

    public SubscriptionChangeMessageReceiver(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(
            queue: Queues.SubscriptionCreate,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        

        channel.QueueBind(queue: Queues.SubscriptionCreate,
            exchange: Exchanges.SubscriptionManagementExchange,
            routingKey: Queues.SubscriptionCreate);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var data = JsonSerializer.Deserialize<Subscription>(message);
            onMessageReceived(data);
        };
        channel.BasicConsume(queue: Queues.SubscriptionCreate,
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }

    private void onMessageReceived(Subscription message)
    {
        Console.WriteLine(message);
    }
}




public record Subscription(string UserId, CryptocurrencySymbol Symbol, double Threshold);
    
public record CryptocurrencySymbol(string Symbol, string BaseAsset, string QuoteAsset);