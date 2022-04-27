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
    
    // This is not RESTful at all, but I did not have time to do it properly 🤷‍
    app.MapPatch("/subscriptions", UpdateSubscriptionParametrized)
      .Produces(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status404NotFound);

    app.MapDelete("/subscriptions/{id}", DeleteSubscription)
      .Produces(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status404NotFound);
    
    // Same
    app.MapDelete("/subscriptions", DeleteSubscriptionParametrized)
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
    return Results.Created($"/subscriptions/{createdSubscription.Value.SubscriptionId}", createdSubscription.Value);
  }

  private async Task<IResult> DeleteSubscription(string id)
  {
    var result = await _subscriptionsService.DeleteSubscription(id);
    return result.IsSuccess
      ? Results.NoContent()
      : Results.NotFound();
  }
  
  private async Task<IResult> DeleteSubscriptionParametrized(string userId, string symbol)
  {
    var subscription = await _subscriptionsService.GetSubscription(userId, symbol);
    return subscription.HasValue
      ? await DeleteSubscription(subscription.Value.SubscriptionId!)
      : Results.NotFound();
  }

  private async Task<IResult> UpdateSubscription(string id, SubscriptionUpdateProperties updateProperties)
  {
    var (isSuccess, _, error) = await _subscriptionsService.Update(id, updateProperties);
    return isSuccess
      ? Results.NoContent()
      : error.Contains("Subscription with specified parameters already exists") 
        ? Results.Conflict() 
        : Results.NotFound();
  }
  
  private async Task<IResult> UpdateSubscriptionParametrized(string userId, string symbol, SubscriptionUpdateProperties updateProperties)
  {
    var subscription = await _subscriptionsService.GetSubscription(userId, symbol);
    return subscription.HasValue
      ? await UpdateSubscription(subscription.Value.SubscriptionId!, updateProperties)
      : Results.NotFound();
  }
}