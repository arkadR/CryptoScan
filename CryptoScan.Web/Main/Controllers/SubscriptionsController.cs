using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using CryptoScan.Web.Main.Models;
using System.Text;
using System.Text.Json;

namespace CryptoScan.Web.Main.Controllers;

[ApiController]
public class SubscriptionsController : ControllerBase
{
  private readonly ConnectionFactory _connectionFactory;

  public SubscriptionsController(ConnectionFactory connectionFactory)
  {
    _connectionFactory = connectionFactory;
  }

  [HttpPost, Route("subscribe")]
  public string Subscribe([FromBody] Subscription subscription) //Actionresult?
  {
    var message = JsonSerializer.Serialize(subscription);

    PublishQueueMessage(message, "subscribe");

    return $"Subscribed to {subscription.symbol.symbol} with a threshold of {subscription.threshold}";
  }

  [HttpDelete, Route("subscribe")]
  public string Unsubscribe([FromBody] Subscription subscription)
  {
    var message = JsonSerializer.Serialize(subscription);

    PublishQueueMessage(message, "unsubscribe");

    return $"Unsubscribed from {subscription.symbol.symbol}";
  }

  private void PublishQueueMessage(string message, string queueName)
  {
    using var connection = _connectionFactory.CreateConnection();
    using var channel = connection.CreateModel();
    channel.QueueDeclare(
        queue: queueName,
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);
    channel.BasicPublish(
        exchange: "",
        routingKey: queueName,
        basicProperties: null,
        body: Encoding.UTF8.GetBytes(message));
  }
}
