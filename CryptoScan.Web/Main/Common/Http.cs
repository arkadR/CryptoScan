using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CryptoScan.Web.Main.Common;

internal static class Http
{
  public static async Task<ActionResult<T>> Get<T>(string url, string? notFoundError = null, string? badRequestError = null)
  {
    try
    {
      string response = await new HttpClient()
      .GetStringAsync(url);

      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      };

      var content = JsonSerializer.Deserialize<T>(response, options);

      return content == null
        ? new NotFoundObjectResult(notFoundError)
        : content;
    }
    catch (Exception ex)
    {
      return new BadRequestObjectResult($"{badRequestError}, \nurl: {url}, \nerror: {ex.Message}");
    }
  }
}
