using Microsoft.AspNetCore.Mvc;

namespace CryptoScan.Web.Main.Extensions;

internal static class ActionResultExtensions
{
  public static bool IsSuccess<T>(this ActionResult<T> result)
    => result as OkObjectResult == null;

  public static ActionResult<TOut> OnSuccess<TIn, TOut>(this ActionResult<TIn> result, Func<TIn, TOut> onSuccessFunc)
  {
    return result.IsSuccess()
      ? onSuccessFunc(result.Value)
      : MapError<TIn, TOut>(result);
  }

  private static ActionResult<TOut> MapError<TIn, TOut>(this ActionResult<TIn> failureResult)
  {
    return failureResult switch
    {
      NoContentResult noContent => noContent,
      BadRequestObjectResult badRequest => badRequest,
      ConflictObjectResult conflict => conflict,
      UnauthorizedObjectResult unauthorized => unauthorized,
      UnprocessableEntityObjectResult unprocessable => unprocessable,
      _ => throw new ArgumentException("Result is not an failure result")
    };
  }
}
