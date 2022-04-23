using CryptoScan.Subscriptions.API.Features;
using CryptoScan.Subscriptions.API.Models;
using MongoDB.Driver;

namespace CryptoScan.Subscriptions.API.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddSubscriptions(this IServiceCollection services, DatabaseSettings databaseSettings)
  {
    var mongoClient = new MongoClient(databaseSettings.ConnectionString);
    var mongoDatabase = mongoClient.GetDatabase(databaseSettings.DatabaseName);
    var subscriptionsCollection =
      mongoDatabase.GetCollection<Subscription>(databaseSettings.SubscriptionsCollectionName);

    services.AddSingleton(mongoClient);
    services.AddSingleton(mongoDatabase);
    services.AddSingleton(subscriptionsCollection);

    services.AddSingleton(new SubscriptionsService(subscriptionsCollection));

    return services;
  }
}