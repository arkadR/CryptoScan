namespace CryptoScan.Subscriptions.API.Models;

public record SubscriptionUpdateProperties(
  CryptocurrencySymbol Symbol, 
  double? Threshold, 
  double? PercentageThreshold,
  TimeRange TimeRange,
  Trend Trend);
