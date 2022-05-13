namespace CryptoScan.Subscriptions.Worker.Options;

public class SubscriptionsApiOptions
{
  public const string SectionName = "SubscriptionsApi";

  public string Url { get; init; } = string.Empty;

}