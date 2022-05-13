namespace CryptoScan.Subscriptions.Worker.Models;

public record TimeRange(
  DateTime? StartDate,
  DateTime? EndDate);