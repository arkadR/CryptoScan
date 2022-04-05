using System.Runtime.Serialization;

namespace CryptoScan.Web.Models;

[DataContract]
public partial class RateLimits
{
  [DataMember(Name = "rateLimitType", EmitDefaultValue = false)]
  public string RateLimitType { get; set; }

  [DataMember(Name = "interval", EmitDefaultValue = false)]
  public string Interval { get; set; }

  [DataMember(Name = "intervalNum", EmitDefaultValue = false)]
  public int? IntervalNum { get; set; }

  [DataMember(Name = "limit", EmitDefaultValue = false)]
  public int? Limit { get; set; }
}
