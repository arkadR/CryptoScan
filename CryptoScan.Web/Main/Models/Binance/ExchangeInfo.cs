using CryptoScan.Web.Models;

namespace CryptoScan.Web.Main.Models;

public partial class ExchangeInfo
{
  public string Timezone { get; set; }

  public long? ServerTime { get; set; }

  public List<RateLimits> RateLimits { get; set; }

  public List<Object> ExchangeFilters { get; set; }

  public List<Symbols> Symbols { get; set; }
}

