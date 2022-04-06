namespace CryptoScan.Web.Models;

public partial class Symbols
{
  public string Symbol { get; set; }

  public string Status { get; set; }

  public string BaseAsset { get; set; }

  public int? BaseAssetPrecision { get; set; }

  public string QuoteAsset { get; set; }

  public int? QuoteAssetPrecision { get; set; }

  public int? BaseCommissionPrecision { get; set; }

  public int? QuoteCommissionPrecision { get; set; }

  public List<string> OrderTypes { get; set; }

  public bool? IcebergAllowed { get; set; }

  public bool? OcoAllowed { get; set; }

  public bool? QuoteOrderQtyMarketAllowed { get; set; }

  public bool? AllowTrailingStop { get; set; }

  public bool? IsSpotTradingAllowed { get; set; }

  public bool? IsMarginTradingAllowed { get; set; }

  public List<Filters> Filters { get; set; }

  public List<string> Permissions { get; set; }
}
