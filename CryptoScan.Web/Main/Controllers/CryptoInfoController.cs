using CryptoScan.Web.Main.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CryptoScan.Web.Main.Extensions;

namespace CryptoScan.Web.Main.Controllers;

[ApiController]
public class CryptoInfoController : ControllerBase
{
  private const string _getExchangeInfoUrl = "https://api1.binance.com/api/v1/exchangeInfo";

  [Route("info/exchange")]
  [HttpGet]
  [ResponseCache(CacheProfileName = "2h")]
  public async Task<ActionResult<ExchangeInfo>> GetAsync()
  {
    string response = await new HttpClient()
      .GetStringAsync(_getExchangeInfoUrl);

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var content = JsonSerializer.Deserialize<ExchangeInfo>(response, options);

    return content == null 
      ? NotFound("Could not fetch exchange info from Binance")
      : content;
  }

  [Route("info/exchange/symbols")]
  [HttpGet]
  [ResponseCache(CacheProfileName = "2h")]
  public async Task<ActionResult<List<Symbol>>> GetSymbolsAsync()
  {
    var exchangeInfoResult = await GetAsync();
    return exchangeInfoResult.IsSuccess()
      ? exchangeInfoResult.Value!.Symbols
        .Select(symbol => new Symbol(symbol.Symbol, symbol.BaseAsset, symbol.QuoteAsset))
        .ToList()
      : NotFound("Could not fetch symbols info from Binance");
  }

  //[Route("info/subscriptions")]
  //[HttpGet]
  //public async Task<IActionResult> GetSubscriptionsInfo(string subscriptionsApiUrl)
  //{
  //  string response = await new HttpClient()
  //    .GetStringAsync(subscriptionsApiUrl);

  //  return JsonSerializer.Deserialize<SubscriptionsInfo>(response);
  //}
}

public record Symbol(string symbol, string baseAsset, string quoteAsset);