using Microsoft.AspNetCore.Mvc;

namespace CryptoScan.Web.Main.Extensions;

internal static class ActionResultExtensions
{
  public static bool IsSuccess<T>(this ActionResult<T> result)
    => result as OkObjectResult == null;
}
