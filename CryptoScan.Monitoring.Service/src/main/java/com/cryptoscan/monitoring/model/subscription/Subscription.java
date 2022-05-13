package com.cryptoscan.monitoring.model.subscription;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class Subscription {
    private String SubscriptionId;
    private String email;
    private CryptocurrencySymbol Symbol;
    private double threshold;
    private double percentageThreshold;
    private TimeRange timeRange;
    private Trend trend;
}
