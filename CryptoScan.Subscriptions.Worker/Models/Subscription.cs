namespace CryptoScan.Subscriptions.Worker.Models;

public record Subscription(string UserId, CryptocurrencySymbol Symbol, double Threshold);