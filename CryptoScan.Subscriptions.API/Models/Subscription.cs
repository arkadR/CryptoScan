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

  public CryptocurrencySymbol Symbol { get; init; }

  public double Threshold { get; init; }
}