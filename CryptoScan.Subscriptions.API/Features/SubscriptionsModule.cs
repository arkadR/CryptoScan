using Carter;
using CryptoScan.Subscriptions.API.Models;
using CSharpFunctionalExtensions;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace CryptoScan.Subscriptions.API.Features;

public class SubscriptionsModule : ICarterModule
{
  private readonly SubscriptionsService _subscriptionsService;

  public SubscriptionsModule(SubscriptionsService subscriptionsService)
  {
    _subscriptionsService = subscriptionsService;
  }

  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/subscriptions", GetSubscriptions)
      .Produces<ICollection<Subscription>>();

    app.MapGet("/subscriptions/{id}", GetSubscription)
      .Produces<Subscription>()
      .Produces(StatusCodes.Status404NotFound);

    app.MapPost("/subscriptions", AddSubscription)
      .Produces<Subscription>(StatusCodes.Status201Created);

    app.MapPatch("/subscriptions/{id}", UpdateSubscription)
      .Produces(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status404NotFound);

    app.MapDelete("/subscriptions/{id}", DeleteSubscription)
      .Produces(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status404NotFound);
  }

  private async Task<IResult> GetSubscriptions()
  {
    var subscriptions = await _subscriptionsService.GetSubscriptions();
    return Results.Ok(subscriptions);
  }

  private async Task<IResult> GetSubscription(string id)
  {
    var (isSuccess, subscription) = await _subscriptionsService.GetSubscription(id);
    return isSuccess
      ? Results.Ok(subscription)
      : Results.NotFound();
  }

  private async Task<IResult> AddSubscription(Subscription subscription)
  {
    var createdSubscription = await _subscriptionsService.CreateSubscription(subscription);
    return Results.Created($"/subscriptions/{createdSubscription.Value.SubscriptionId}", createdSubscription);
  }

  private async Task<IResult> DeleteSubscription(string id)
  {
    var result = await _subscriptionsService.DeleteSubscription(id);
    return result.IsSuccess
      ? Results.NoContent()
      : Results.NotFound();
  }

  private async Task<IResult> UpdateSubscription(string id, SubscriptionUpdateProperties updateProperties)
  {
    var result = await _subscriptionsService.Update(id, updateProperties);
    return result.IsSuccess
      ? Results.NoContent()
      : Results.NotFound();
  }
}