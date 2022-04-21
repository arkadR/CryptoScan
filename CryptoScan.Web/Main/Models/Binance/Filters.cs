namespace CryptoScan.Web.Models;

public class Filters
{
  public string FilterType { get; init; }

  public string MinPrice { get; init; }

  public string MaxPrice { get; init; }

  public string TickSize { get; init; }
}
