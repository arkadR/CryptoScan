var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("subscriptions", () => new List<Subscription>
{
  new Subscription(
      userId: "testUserId",
      symbol: new Symbol("BTCETH", "BTC", "ETH"),
      timeRange: new TimeRange(DateTime.Now, DateTime.Now.AddYears(2)),
      threshold: 3.2,
      percentageThreshold: 50),
  new Subscription(
      userId: "testUserId",
      symbol: new Symbol("LTCBTC", "LTC", "BTC"),
      timeRange: new TimeRange(DateTime.Now, DateTime.Now.AddMonths(4)),
      threshold: 0.975,
      percentageThreshold: 30,
      trend: Trend.Up)
});

app.Run();

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