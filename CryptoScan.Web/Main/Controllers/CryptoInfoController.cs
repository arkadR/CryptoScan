﻿using CryptoScan.Web.Main.Models;
using Microsoft.AspNetCore.Mvc;
using CryptoScan.Web.Main.Extensions;
using CryptoScan.Web.Main.Common;

namespace CryptoScan.Web.Main.Controllers;

[ApiController]
public class CryptoInfoController : ControllerBase
{
  private readonly string _getExchangeInfoUrl;
  private readonly string _subscriptionsApiUrl;

  public CryptoInfoController(IConfiguration configuration)
  {
    _getExchangeInfoUrl = configuration["Binance:ExchangeInfoUrl"]!;
    var apiHost = configuration["SubscriptionsApi:HostName"];
    var apiEndpoint = configuration["SubscriptionsApi:Endpoints:Subscriptions"];
    _subscriptionsApiUrl = $"http://{apiHost}/{apiEndpoint}";
  }

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

  [HttpGet, Route("info/subscriptions")]
  public async Task<ActionResult<List<Subscription>>> GetSubscriptionsInfo()
  {
    return await Http.Get<List<Subscription>>(
     url: _subscriptionsApiUrl,
     notFoundError: "Could not fetch exchange info from subscriptions api",
     badRequestError: "Subscriptions api server not avilable");
  }
}