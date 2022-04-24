namespace CryptoScan.Web.Main.Models;

public record Subscription(string email, Symbol symbol, double threshold);

public record Symbol(string symbol, string baseAsset, string quoteAsset);