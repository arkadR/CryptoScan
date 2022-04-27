using CryptoScan.Subscriptions.Worker.Models;

namespace CryptoScan.Subscriptions.Worker;

public class SubscriptionService : ISubscriptionService
{
  private readonly HttpClient _httpClient;

  public SubscriptionService(HttpClient httpClient)
  {
    _httpClient = httpClient;
    _httpClient.BaseAddress = new Uri("http://localhost:8001");
  }

  public async Task<bool> Create(Subscription subscription)
  {
    var result = await _httpClient.PostAsJsonAsync("subscriptions", subscription);
    return result.IsSuccessStatusCode;
  }

  public async Task<bool> Update(Subscription subscription)
  {
    var result = await _httpClient.PatchAsJsonAsync(
      $"subscriptions?userId={subscription.UserId}&symbol={subscription.Symbol.Symbol}", 
      subscription);
    var r = await result.Content.ReadAsStringAsync();
    return result.IsSuccessStatusCode;
  }

  public async Task<bool> Delete(Subscription subscription)
  {
    var result = await _httpClient.DeleteAsync(
      $"subscriptions?userId={subscription.UserId}&symbol={subscription.Symbol.Symbol}");
    return result.IsSuccessStatusCode;
  }
}