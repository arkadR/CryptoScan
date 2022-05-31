namespace CryptoScan.Subscriptions.Worker.Models;

public record SubscriptionUpdateProperties(
  CryptocurrencySymbol Symbol, 
  double? Threshold, 
  double? PercentageThreshold,
  TimeRange TimeRange,
  Trend Trend);
