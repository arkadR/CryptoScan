namespace CryptoScan.Web.Main.Models;

public record Subscription(string UserId, CryptocurrencySymbol Symbol, double Threshold);
    
public record CryptocurrencySymbol(string Symbol, string BaseAsset, string QuoteAsset);