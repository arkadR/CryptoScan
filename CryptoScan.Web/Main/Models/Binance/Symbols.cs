namespace CryptoScan.Web.Models;

public class Symbols
{
  public string Symbol { get; init; }

  public string Status { get; init; }

  public string BaseAsset { get; init; }

  public int? BaseAssetPrecision { get; init; }

  public string QuoteAsset { get; init; }

  public int? QuoteAssetPrecision { get; init; }

  public int? BaseCommissionPrecision { get; init; }

  public int? QuoteCommissionPrecision { get; init; }

  public List<string> OrderTypes { get; init; }

  public bool? IcebergAllowed { get; init; }

  public bool? OcoAllowed { get; init; }

  public bool? QuoteOrderQtyMarketAllowed { get; init; }

  public bool? AllowTrailingStop { get; init; }

  public bool? IsSpotTradingAllowed { get; init; }

  public bool? IsMarginTradingAllowed { get; init; }

  public List<Filters> Filters { get; init; }

  public List<string> Permissions { get; init; }
}
