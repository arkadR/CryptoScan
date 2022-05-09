namespace CryptoScan.Subscriptions.Worker.Models;

public record CryptocurrencySymbol(
  string Symbol,
  string BaseAsset,
  string QuoteAsset);
