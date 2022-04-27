using CryptoScan.Subscriptions.Worker.Models;

namespace CryptoScan.Subscriptions.Worker;

public interface ISubscriptionService
{
  public Task<bool> Create(Subscription subscription);
  public Task<bool> Update(Subscription subscription);
  public Task<bool> Delete(Subscription subscription);
}