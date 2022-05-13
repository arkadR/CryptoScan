namespace CryptoScan.Web.Main.Models;

public record Subscription(
  string Email, 
  CryptocurrencySymbol Symbol,
  TimeRange TimeRange, 
  double? Threshold, 
  double? PercentageThreshold,
  Trend Trend = Trend.Unspecified);

public record CryptocurrencySymbol(
  string Symbol, 
  string BaseAsset, 
  string QuoteAsset);

public record TimeRange(
  DateTime? StartDate, 
  DateTime? EndDate);

public enum Trend
{
  Unspecified,
  Up,
  Down
}
