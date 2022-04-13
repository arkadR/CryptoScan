using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("subscriptions", () => new List<Subscription>
{
  new Subscription("test.mail@temp.com", new Symbol("ETHBTC", "ETH", "BTC"), 2.3345),
  new Subscription("test.mail@temp.com", new Symbol("LTCBTC", "LTC", "BTC"), 0.975)
});

app.Run();

public record Subscription(string email, Symbol symbol, double threshold);

public record Symbol(string symbol, string baseAsset, string quoteAsset);