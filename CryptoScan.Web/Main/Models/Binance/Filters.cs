using System.Runtime.Serialization;

namespace CryptoScan.Web.Models;

[DataContract]
public partial class Filters
{
  [DataMember(Name = "filterType", EmitDefaultValue = false)]
  public string FilterType { get; set; }

  [DataMember(Name = "minPrice", EmitDefaultValue = false)]
  public string MinPrice { get; set; }

  [DataMember(Name = "maxPrice", EmitDefaultValue = false)]
  public string MaxPrice { get; set; }

  [DataMember(Name = "tickSize", EmitDefaultValue = false)]
  public string TickSize { get; set; }
}
