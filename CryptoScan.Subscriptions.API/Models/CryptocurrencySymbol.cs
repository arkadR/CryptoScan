namespace CryptoScan.Subscriptions.API.Models;

public record CryptocurrencySymbol(
  string Symbol,
  string BaseAsset,
  string QuoteAsset);