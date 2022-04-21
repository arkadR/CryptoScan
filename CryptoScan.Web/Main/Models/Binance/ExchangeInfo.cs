using CryptoScan.Web.Models;

namespace CryptoScan.Web.Main.Models;

public class ExchangeInfo
{
  public string Timezone { get; init; }

  public long? ServerTime { get; init; }

  public List<RateLimits> RateLimits { get; init; }

  public List<Object> ExchangeFilters { get; init; }

  public List<Symbols> Symbols { get; init; }
}

