using Microsoft.AspNetCore.Mvc;

namespace CryptoScan.Web.Main.Common;

public interface IHttp
{
  Task<ActionResult<T>> Get<T>(string url, string? notFoundError, string? badRequestError);
}
