namespace CryptoScan.Subscriptions.Worker.Options;

public class RabbitMqOptions
{
  public const string SectionName = "RabbitMQ";
    
  public string HostName { get; init; } = string.Empty;
  public int Port { get; init; }
  public string UserName { get; init; } = string.Empty;
  public string Password { get; init; } = string.Empty;
}