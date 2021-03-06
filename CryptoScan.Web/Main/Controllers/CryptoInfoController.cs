using CryptoScan.Web.Main.Models;
using Microsoft.AspNetCore.Mvc;
using CryptoScan.Web.Main.Extensions;
using CryptoScan.Web.Main.Common;
using CryptoScan.Web.Main.Models.Binance;

namespace CryptoScan.Web.Main.Controllers;

[ApiController]
public class CryptoInfoController : ControllerBase
{
  private readonly string _getExchangeInfoUrl;
  private readonly string _subscriptionsApiUrl;

  private readonly IHttp _http;

  public CryptoInfoController(IConfiguration configuration, IHttp http)
  {
    _http = http;
    _getExchangeInfoUrl = configuration["Binance:ExchangeInfoUrl"]!;
    var apiHost = configuration["SubscriptionsApi:HostName"];
    var apiEndpoint = configuration["SubscriptionsApi:Endpoints:Subscriptions"];
    _subscriptionsApiUrl = $"http://{apiHost}/{apiEndpoint}";
  }

  [HttpGet, Route("info/exchange")]
  [ResponseCache(CacheProfileName = "2h")]
  public async Task<ActionResult<ExchangeInfo>> GetExchangeInfoAsync()
  {
    return await _http.Get<ExchangeInfo>(
      url: _getExchangeInfoUrl, 
      notFoundError: "Could not fetch exchange info from Binance",
      badRequestError: "Binance server not available");
  }

  [HttpGet, Route("info/exchange/symbols")]
  [ResponseCache(CacheProfileName = "2h")]
  public async Task<ActionResult<List<CryptocurrencySymbol>>> GetSymbolsAsync()
  {
    return (await GetExchangeInfoAsync())
      .OnSuccess(exchangeInfo => exchangeInfo.Symbols
        .Select(symbol => new CryptocurrencySymbol(symbol.Symbol, symbol.BaseAsset, symbol.QuoteAsset))
        .ToList());
  }

  [HttpGet, Route("info/exchange/symbols/{quoteAsset}")]
  [ResponseCache(CacheProfileName = "2h")]
  public async Task<ActionResult<List<CryptocurrencySymbol>>> GetByQuoteAssetAsync(string quoteAsset)
  {
    return (await GetSymbolsAsync())
      .OnSuccess(symbols => symbols
        .Where(symbol => symbol.QuoteAsset == quoteAsset.ToUpper())
        .ToList());
  }

  [HttpGet, Route("info/subscriptions")]
  public async Task<ActionResult<List<Subscription>>> GetSubscriptionsInfo(string userId)
  {
    return await _http.Get<List<Subscription>>(
     url: $"{_subscriptionsApiUrl}?userId={userId}",
     notFoundError: "Could not fetch exchange info from subscriptions api",
     badRequestError: "Subscriptions api server not available");
  }
}
