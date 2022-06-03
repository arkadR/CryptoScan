package com.cryptoscan.monitoring.model.subscription;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class Subscription {
    private String SubscriptionId;
    private String UserId;
    private String Email;
    private CryptocurrencySymbol Symbol;
    private Double threshold;
    private Double percentageThreshold;
    private TimeRange timeRange;
    private Trend trend;
}
