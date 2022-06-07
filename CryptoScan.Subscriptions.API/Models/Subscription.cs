using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CryptoScan.Subscriptions.API.Models;

public record Subscription
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  [StringLength(24, MinimumLength = 24)]
  public string? SubscriptionId { get; init; }

  public string UserId { get; init; }
  
  public string Email { get; init; }

  public CryptocurrencySymbol Symbol { get; init; }

  public double? Threshold { get; init; }
  
  public double? PercentageThreshold { get; init; }

  public TimeRange TimeRange { get; init; }

  public Trend Trend { get; init; } = Trend.Unspecified;
}
