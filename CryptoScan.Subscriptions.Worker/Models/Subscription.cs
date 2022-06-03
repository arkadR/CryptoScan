using System.ComponentModel.DataAnnotations;

namespace CryptoScan.Subscriptions.Worker.Models;

public record Subscription(
  string? SubscriptionId, 
  string UserId,
  string Email,
  CryptocurrencySymbol Symbol,
  double? Threshold,
  double? PercentageThreshold,
  TimeRange TimeRange,
  Trend Trend);