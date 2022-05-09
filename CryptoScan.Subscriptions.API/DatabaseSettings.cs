namespace CryptoScan.Subscriptions.API;

public class DatabaseSettings
{
  public string ConnectionString { get; init; } = null!;

  public string DatabaseName { get; init; } = null!;

  public string SubscriptionsCollectionName { get; init; } = null!;
}