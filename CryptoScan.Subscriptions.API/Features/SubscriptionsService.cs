using CryptoScan.Subscriptions.API.Extensions;
using CryptoScan.Subscriptions.API.Models;
using CSharpFunctionalExtensions;
using MongoDB.Driver;

namespace CryptoScan.Subscriptions.API.Features;

public class SubscriptionsService
{
  private readonly IMongoCollection<Subscription> _subscriptionsCollection;

  public SubscriptionsService(IMongoCollection<Subscription> subscriptionsCollection)
  {
    _subscriptionsCollection = subscriptionsCollection;
  }

  public async Task<List<Subscription>> GetSubscriptions()
  {
    return await _subscriptionsCollection
      .Find(_ => true)
      .ToListAsync();
  }

  public async Task<Maybe<Subscription>> GetSubscription(string id)
  {
    return (await _subscriptionsCollection
        .Find(x => x.SubscriptionId == id)
        .FirstOrDefaultAsync())
      .ToMaybe();
  }

  public async Task<Result<Subscription>> CreateSubscription(Subscription subscription)
  {
    var subscriptionToInsert = subscription with { }; // Creating a clone because MongoDB mutates the object on Insert
    await _subscriptionsCollection
      .InsertOneAsync(subscription);

    return subscriptionToInsert;
  }

  // This is wank because MongoDB is wank
  public async Task<Result> Update(string id, SubscriptionUpdateProperties updateProperties)
  {
    var subscription = await GetSubscription(id);
    if (subscription.HasValue == false)
      return Result.Failure($"Subscription with id [{id}] not found.");

    var filter = Builders<Subscription>.Filter
      .Eq(p => p.SubscriptionId, id);
    var update = Builders<Subscription>.Update
      .Set(p => p.Symbol, updateProperties.Symbol)
      .Set(p => p.Threshold, updateProperties.Threshold);

    await _subscriptionsCollection.UpdateOneAsync(
      filter,
      update);

    return Result.Success();
  }

  public async Task<Result> DeleteSubscription(string id)
  {
    var subscription = await GetSubscription(id);
    if (subscription.HasValue == false)
      return Result.Failure($"Subscription with id [{id}] not found.");

    var filter = Builders<Subscription>.Filter.Eq(p => p.SubscriptionId, id);
    await _subscriptionsCollection.DeleteOneAsync(filter);
    return Result.Success();
  }
}