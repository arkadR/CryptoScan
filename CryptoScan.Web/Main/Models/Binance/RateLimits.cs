namespace CryptoScan.Web.Models;

public class RateLimits
{
  public string RateLimitType { get; init; }

  public string Interval { get; init; }

  public int? IntervalNum { get; init; }

  public int? Limit { get; init; }
}
