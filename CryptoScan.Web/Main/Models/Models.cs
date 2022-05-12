namespace CryptoScan.Web.Main.Models;

public record Subscription(
  string userId, 
  Symbol symbol,
  TimeRange timeRange, 
  double? threshold, 
  double? percentageThreshold,
  Trend trend = Trend.Unspecified);

public record Symbol(
  string symbol, 
  string baseAsset, 
  string quoteAsset);

public record TimeRange(
  DateTime? startDate, 
  DateTime? endDate);

public enum Trend
{
  Unspecified,
  Up,
  Down
}