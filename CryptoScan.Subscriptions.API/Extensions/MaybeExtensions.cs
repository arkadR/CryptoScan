using CSharpFunctionalExtensions;

namespace CryptoScan.Subscriptions.API.Extensions;

public static class MaybeExtensions
{
  public static Maybe<T> ToMaybe<T>(this T? value)
  {
    return value != null
      ? Maybe<T>.From(value)
      : Maybe<T>.None;
  }
}