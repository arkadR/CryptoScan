using System.Text;
using System.Text.Json;
using CryptoScan.Constants;
using CryptoScan.Subscriptions.Worker.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CryptoScan.Subscriptions.Worker;

public class SubscriptionChangeMessageReceiver : BackgroundService
{
  private readonly IConnection _connection;  
  private readonly IModel _channel; 

  public SubscriptionChangeMessageReceiver(IConnectionFactory connectionFactory)
  {
    _connection = connectionFactory.CreateConnection();
    _channel = _connection.CreateModel();
    _channel.ExchangeDeclare(exchange: Exchanges.SubscriptionManagementExchange, ExchangeType.Fanout);
    _channel.QueueDeclare(
      queue: Queues.SubscriptionCreate,
      durable: false,
      exclusive: false,
      autoDelete: false,
      arguments: null);
        
    _channel.QueueBind(queue: Queues.SubscriptionCreate,
      exchange: Exchanges.SubscriptionManagementExchange,
      routingKey: Queues.SubscriptionCreate);
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    stoppingToken.ThrowIfCancellationRequested();

    var consumer = new EventingBasicConsumer(_channel);
    consumer.Received += (model, ea) =>
    {
      var body = ea.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      var data = JsonSerializer.Deserialize<Subscription>(message);
      onMessageReceived(data);
    };
    _channel.BasicConsume(queue: Queues.SubscriptionCreate,
      autoAck: true,
      consumer: consumer);

    return Task.CompletedTask;
  }

  private void onMessageReceived(Subscription message)
  {
    Console.WriteLine(message);
  }

  public override void Dispose()
  {
    _channel.Dispose();
    _connection.Dispose();
    base.Dispose();
  }
}