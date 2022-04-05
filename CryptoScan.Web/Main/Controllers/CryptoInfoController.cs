using CryptoScan.Web.Main.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CryptoScan.Web.Main.Controllers;

[ApiController]
public class CryptoInfoController : ControllerBase
{
  private const string _getExchangeInfoUrl = "https://api1.binance.com/api/v1/exchangeInfo";

  [Route("info/exchange")]
  [HttpGet]
  [ResponseCache(CacheProfileName = "2h")]
  public async Task<IActionResult> GetAsync()
  {
    string response = await new HttpClient()
      .GetStringAsync(_getExchangeInfoUrl);

    var content = JsonSerializer.Deserialize<ExchangeInfo>(response);

    return content == null 
      ? NotFound("Could not fetch exchange info from Binance")
      : Ok(content);
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
