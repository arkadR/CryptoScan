using System.Net;
using System.IO;
using Newtonsoft.Json;

internal class CurrenciesInfoFetcher
{

  public static List<CurrencyInfo> GetCurrenciesInfo()
  {
    string response = new WebClient().DownloadString("https://api1.binance.com/api/v1/exchangeInfo");

    var jsonObj = JsonConvert.DeserializeObject<JObject>(response);
    return jsonObj.Value<JArray>("symbols")
      .ToObject<List<CurrencyInfo>>();
  }
}

internal record CurrencyInfo(string symbol, string baseAsset, string quoteAsset);
