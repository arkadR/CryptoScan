using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using CryptoScan.Web.Main.Models.Queue;

namespace CryptoScan.Web.Main.Controllers;

[ApiController]
public class TestController : ControllerBase
{
  private readonly ConnectionFactory _connectionFactory;

  public TestController(ConnectionFactory connectionFactory)
  {
    _connectionFactory = connectionFactory;
  }

  [Route("test")]
  [HttpPost]
  public string Test()
  {
    var subscription = new Subscription("test@gmail.com", "Bitcoin");
    var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(subscription));

    using var connection = _connectionFactory.CreateConnection();
    using var channel = connection.CreateModel();
    channel.QueueDeclare(
        queue: "subscriptionRequests",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);
    channel.BasicPublish(
        exchange: "",
        routingKey: "subscriptionRequests",
        basicProperties: null,
        body: message);
    return "Message queued";
  }
}
