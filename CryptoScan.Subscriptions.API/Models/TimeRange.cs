namespace CryptoScan.Subscriptions.API.Models;

public record TimeRange(
  DateTime? StartDate,
  DateTime? EndDate);