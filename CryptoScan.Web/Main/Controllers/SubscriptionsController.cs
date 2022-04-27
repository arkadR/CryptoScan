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

  [HttpPost, Route("subscriptions/subscribe")]
  public string Subscribe([FromBody] Subscription subscription) //Actionresult?
  {
    var message = JsonSerializer.Serialize(subscription);

    PublishQueueMessage(message, Constants.Queues.SubscriptionCreate);

    return $"Subscribed to {subscription.Symbol.Symbol} with a threshold of {subscription.Threshold}";
  }

  [HttpPatch, Route("subscriptions/update")]
  public string Update([FromBody] Subscription subscription)
  {
    var message = JsonSerializer.Serialize(subscription);

    PublishQueueMessage(message, Constants.Queues.SubscriptionUpdate);

    return $"Updated threshold for {subscription.Symbol.Symbol} with a value of {subscription.Threshold}";
  }

  [HttpDelete, Route("subscriptions/unsubscribe")]
  public string Unsubscribe([FromBody] Subscription subscription)
  {
    var message = JsonSerializer.Serialize(subscription);

    PublishQueueMessage(message, Constants.Queues.SubscriptionDelete);

    return $"Unsubscribed from {subscription.Symbol.Symbol}";
  }

  private void PublishQueueMessage(string message, string queueName)
  {
    using var connection = _connectionFactory.CreateConnection();
    using var channel = connection.CreateModel();
    channel.ExchangeDeclare(exchange: Constants.Exchanges.SubscriptionManagementExchange, ExchangeType.Direct);
    channel.QueueDeclare(
        queue: queueName,
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);
    channel.QueueBind(queueName, Constants.Exchanges.SubscriptionManagementExchange, queueName);
    channel.BasicPublish(
        exchange: Constants.Exchanges.SubscriptionManagementExchange,
        routingKey: queueName,
        basicProperties: null,
        body: Encoding.UTF8.GetBytes(message));
    channel.Close();
  }
}
