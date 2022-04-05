using System.Runtime.Serialization;

namespace CryptoScan.Web.Main.Models;

[DataContract]
public partial class ExchangeInfo
{
  [DataMember(Name = "timezone", EmitDefaultValue = false)]
  public string Timezone { get; set; }

  [DataMember(Name = "serverTime", EmitDefaultValue = false)]
  public long? ServerTime { get; set; }

  [DataMember(Name = "rateLimits", EmitDefaultValue = false)]
  public List<RateLimits> RateLimits { get; set; }

  [DataMember(Name = "exchangeFilters", EmitDefaultValue = false)]
  public List<Object> ExchangeFilters { get; set; }

  [DataMember(Name = "symbols", EmitDefaultValue = false)]
  public List<Symbols> Symbols { get; set; }
}

