package com.cryptoscan.monitoring.service;

import com.cryptoscan.monitoring.model.message.CryptoChangesMessage;
import com.cryptoscan.monitoring.model.subscription.Subscription;
import com.cryptoscan.monitoring.model.subscription.Trend;
import com.cryptoscan.monitoring.model.ticker.Ticker;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

@Service
public class MonitoringService {

    public List<CryptoChangesMessage> getMessages(Map<Subscription, Ticker> subscriptionsToTickers) {
        List<Subscription> subscriptions = subscriptionsToTickers.entrySet().stream()
                .filter(e -> isThresholdExceeded(e.getKey(), e.getValue()))
                .map(Map.Entry::getKey)
                .collect(Collectors.toList());
        return subscriptions.stream()
                .map(CryptoChangesMessage::fromSubscription)
                .collect(Collectors.toList());
    }

    private boolean isThresholdExceeded(Subscription subscription, Ticker ticker) {
        Double threshold = subscription.getThreshold();
        double priceChange = ticker.getPriceChange();
        Double percentageThreshold = subscription.getPercentageThreshold();
        double percentagePriceChange = ticker.getPriceChangePercent();
        boolean isUpThresholdExceeded = subscription.getTrend() == Trend.Up
                && isUpThresholdExceeded(priceChange, threshold, percentagePriceChange, percentageThreshold);
        boolean isDownThresholdExceeded = subscription.getTrend() == Trend.Down
                && isDownThresholdExceeded(priceChange, threshold, percentagePriceChange, percentageThreshold);
        return isUpThresholdExceeded || isDownThresholdExceeded;
    }

    private boolean isUpThresholdExceeded(double priceChange, Double threshold,
                                          double percentagePriceChange, Double percentageThreshold) {
        if (threshold != null) {
            return priceChange >= threshold;
        } else if (percentageThreshold != null) {
            return percentagePriceChange >= percentageThreshold;
        }
        return false;
    }

    private boolean isDownThresholdExceeded(double priceChange, Double threshold,
                                            double percentagePriceChange, Double percentageThreshold) {
        if (threshold != null) {
            return priceChange <= -threshold;
        } else if (percentageThreshold != null) {
            return percentagePriceChange <= -percentageThreshold;
        }
        return false;
    }
    
}
