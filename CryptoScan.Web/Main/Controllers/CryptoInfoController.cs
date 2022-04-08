using CryptoScan.Web.Main.Models;
using Microsoft.AspNetCore.Mvc;
using CryptoScan.Web.Main.Extensions;
using CryptoScan.Web.Main.Common;

namespace CryptoScan.Web.Main.Controllers;

[ApiController]
public class CryptoInfoController : ControllerBase
{
  private const string _getExchangeInfoUrl = "https://api1.binance.com/api/v1/exchangeInfo";

  [HttpGet, Route("info/exchange")]
  [ResponseCache(CacheProfileName = "2h")]
  public async Task<ActionResult<ExchangeInfo>> GetExchangeInfoAsync()
  {
    return await Http.Get<ExchangeInfo>(
      url: _getExchangeInfoUrl, 
      notFoundError: "Could not fetch exchange info from Binance",
      badRequestError: "Bianance server not avilable");
  }

  [HttpGet, Route("info/exchange/symbols")]
  [ResponseCache(CacheProfileName = "2h")]
  public async Task<ActionResult<List<Symbol>>> GetSymbolsAsync()
  {
    return (await GetExchangeInfoAsync())
      .OnSuccess(exchangeInfo => exchangeInfo.Symbols
        .Select(symbol => new Symbol(symbol.Symbol, symbol.BaseAsset, symbol.QuoteAsset))
        .ToList());
  }

  [HttpGet, Route("info/exchange/symbols/{quoteAsset}")]
  [ResponseCache(CacheProfileName = "2h")]
  public async Task<ActionResult<List<Symbol>>> GetByQuoteAssetAsync(string quoteAsset)
  {
    return (await GetSymbolsAsync())
      .OnSuccess(symbols => symbols
        .Where(symbol => symbol.quoteAsset == quoteAsset.ToUpper())
        .ToList());
  }
}

public record Symbol(string symbol, string baseAsset, string quoteAsset);