namespace CryptoScan.Web.Models;

public partial class RateLimits
{
  public string RateLimitType { get; set; }

  public string Interval { get; set; }

  public int? IntervalNum { get; set; }

  public int? Limit { get; set; }
}
