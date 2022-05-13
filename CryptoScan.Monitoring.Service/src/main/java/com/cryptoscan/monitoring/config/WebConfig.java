package com.cryptoscan.monitoring.config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.reactive.function.client.WebClient;

@Configuration
public class WebConfig {

    @Value("${subscription.api.baseurl}")
    private String subscriptionApiBaseUrl;

    @Value("${binance.baseurl}")
    private String binanceBaseUrl;

    @Bean(name="subscriptionsApiWebClient")
    public WebClient subscriptionsApiWebClient() {
        return WebClient.builder()
                .baseUrl(subscriptionApiBaseUrl)
                .build();
    }

    @Bean(name="binanceWebClient")
    public WebClient binanceWebClient() {
        return WebClient.builder()
                .baseUrl(binanceBaseUrl)
                .build();
    }

}

