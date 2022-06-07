using System.Text;
using System.Text.Json;
using CryptoScan.Constants;
using CryptoScan.Subscriptions.Worker.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CryptoScan.Subscriptions.Worker;

public class SubscriptionChangeMessageReceiver : BackgroundService
{
  private readonly IConnection _connection;  
  private readonly IModel _channel;
  private readonly ISubscriptionService _subscriptionService;
  private readonly ILogger _logger;

  public SubscriptionChangeMessageReceiver(
    IConnectionFactory connectionFactory, 
    ISubscriptionService subscriptionService, 
    ILogger<SubscriptionChangeMessageReceiver> logger)
  {
    _subscriptionService = subscriptionService;
    _logger = logger;
    _connection = connectionFactory.CreateConnection();
    _channel = _connection.CreateModel();

    DeclareExchangeAndQueues();
  }

  private void DeclareExchangeAndQueues()
  {
    _channel.ExchangeDeclare(exchange: Exchanges.SubscriptionManagementExchange, ExchangeType.Direct);

    foreach (var queueName in new List<string> {
                 Queues.SubscriptionCreate, 
                 Queues.SubscriptionUpdate, 
                 Queues.SubscriptionDelete})
    {
      _channel.QueueDeclare(
        queue: queueName,
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);
    }
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    stoppingToken.ThrowIfCancellationRequested();

    var subscriptionCreateConsumer = new EventingBasicConsumer(_channel);
    subscriptionCreateConsumer.Received += 
      async (_, ea) => 
        await OnSubscriptionCreateRequestReceived(DeserializeSubscription(ea), ea.DeliveryTag);
    
    var subscriptionUpdateConsumer = new EventingBasicConsumer(_channel);
    subscriptionUpdateConsumer.Received += 
      async (_, ea) => 
        await OnSubscriptionUpdateRequestReceived(DeserializeSubscription(ea), ea.DeliveryTag);
    
    var subscriptionDeleteConsumer = new EventingBasicConsumer(_channel);
    subscriptionDeleteConsumer.Received += 
      async (_, ea) => 
        await OnSubscriptionDeleteRequestReceived(DeserializeSubscription(ea), ea.DeliveryTag);
    
    _channel.BasicConsume(Queues.SubscriptionCreate, false, subscriptionCreateConsumer);
    _channel.BasicConsume(Queues.SubscriptionUpdate, false, subscriptionUpdateConsumer);
    _channel.BasicConsume(Queues.SubscriptionDelete, false, subscriptionDeleteConsumer);

    return Task.CompletedTask;
  }

  private async Task OnSubscriptionCreateRequestReceived(Subscription message, ulong deliveryTag)
  {
    ArgumentNullException.ThrowIfNull(message);
    _logger.LogInformation("Processing create request {Message}", message);
    var success = await _subscriptionService.Create(message);
    if (success)
      _channel.BasicAck(deliveryTag, false);
    else 
      _channel.BasicNack(deliveryTag, false, false);
  }
  
  private async Task OnSubscriptionUpdateRequestReceived(Subscription message, ulong deliveryTag)
  {
    ArgumentNullException.ThrowIfNull(message);
    _logger.LogInformation("Processing update request {Message}", message);
    var success = await _subscriptionService.Update(message);
    if (success)
      _channel.BasicAck(deliveryTag, false);
    else 
      _channel.BasicNack(deliveryTag, false, false);
  }
  
  private async Task OnSubscriptionDeleteRequestReceived(Subscription message, ulong deliveryTag)
  {
    ArgumentNullException.ThrowIfNull(message);
    _logger.LogInformation("Processing delete request {Message}", message);
    var success = await _subscriptionService.Delete(message);
    if (success)
      _channel.BasicAck(deliveryTag, false);
    else 
      _channel.BasicNack(deliveryTag, false, false);
  }

  private static Subscription DeserializeSubscription(BasicDeliverEventArgs ea)
  {
    return JsonSerializer.Deserialize<Subscription>(
      Encoding.UTF8.GetString(
        ea.Body.ToArray()
      )
    )!;
  }

  public override void Dispose()
  {
    _channel.Dispose();
    _connection.Dispose();
    base.Dispose();
  }
}