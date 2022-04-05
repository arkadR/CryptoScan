using System.Runtime.Serialization;

namespace CryptoScan.Web.Models;

[DataContract]
public partial class Symbols
{
  [DataMember(Name = "symbol", EmitDefaultValue = false)]
  public string Symbol { get; set; }

  [DataMember(Name = "status", EmitDefaultValue = false)]
  public string Status { get; set; }

  [DataMember(Name = "baseAsset", EmitDefaultValue = false)]
  public string BaseAsset { get; set; }

  [DataMember(Name = "baseAssetPrecision", EmitDefaultValue = false)]
  public int? BaseAssetPrecision { get; set; }

  [DataMember(Name = "quoteAsset", EmitDefaultValue = false)]
  public string QuoteAsset { get; set; }

  [DataMember(Name = "quoteAssetPrecision", EmitDefaultValue = false)]
  public int? QuoteAssetPrecision { get; set; }

  [DataMember(Name = "baseCommissionPrecision", EmitDefaultValue = false)]
  public int? BaseCommissionPrecision { get; set; }

  [DataMember(Name = "quoteCommissionPrecision", EmitDefaultValue = false)]
  public int? QuoteCommissionPrecision { get; set; }

  [DataMember(Name = "orderTypes", EmitDefaultValue = false)]
  public List<string> OrderTypes { get; set; }

  [DataMember(Name = "icebergAllowed", EmitDefaultValue = false)]
  public bool? IcebergAllowed { get; set; }

  [DataMember(Name = "ocoAllowed", EmitDefaultValue = false)]
  public bool? OcoAllowed { get; set; }

  [DataMember(Name = "quoteOrderQtyMarketAllowed", EmitDefaultValue = false)]
  public bool? QuoteOrderQtyMarketAllowed { get; set; }

  [DataMember(Name = "allowTrailingStop", EmitDefaultValue = false)]
  public bool? AllowTrailingStop { get; set; }

  [DataMember(Name = "isSpotTradingAllowed", EmitDefaultValue = false)]
  public bool? IsSpotTradingAllowed { get; set; }

  [DataMember(Name = "isMarginTradingAllowed", EmitDefaultValue = false)]
  public bool? IsMarginTradingAllowed { get; set; }

  [DataMember(Name = "filters", EmitDefaultValue = false)]
  public List<Filters> Filters { get; set; }

  [DataMember(Name = "permissions", EmitDefaultValue = false)]
  public List<string> Permissions { get; set; }
}
