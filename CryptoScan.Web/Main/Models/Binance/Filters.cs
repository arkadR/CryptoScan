namespace CryptoScan.Web.Models;

public partial class Filters
{
  public string FilterType { get; set; }

  public string MinPrice { get; set; }

  public string MaxPrice { get; set; }

  public string TickSize { get; set; }
}
