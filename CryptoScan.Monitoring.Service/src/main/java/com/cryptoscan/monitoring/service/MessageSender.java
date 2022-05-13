package com.cryptoscan.monitoring.service;

import com.cryptoscan.monitoring.model.message.CryptoChangesMessage;
import com.cryptoscan.monitoring.model.subscription.CryptocurrencySymbol;
import com.cryptoscan.monitoring.model.subscription.Subscription;
import com.cryptoscan.monitoring.model.ticker.Ticker;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.amqp.core.Queue;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;
import org.springframework.web.reactive.function.client.WebClient;

import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
@Slf4j
public class MessageSender {

    private static final int SCHEDULE_INTERVAL = 86400000;

    private final RabbitTemplate rabbitTemplate;

    private final Queue queue;

    @Qualifier("subscriptionsApiWebClient")
    private final WebClient subscriptionsApiWebClient;

    @Qualifier("binanceWebClient")
    private final WebClient binanceWebClient;

    @Scheduled(fixedRate = SCHEDULE_INTERVAL)
    public void monitoringTask() {
        List<Subscription> subscriptions = getSubscriptions();
        subscriptions.forEach(System.out::println);
        List<Ticker> tickers = subscriptions.stream()
                .map(subscription -> getCryptoChanges(subscription.getSymbol()))
                .collect(Collectors.toList());
        tickers.forEach(System.out::println);
    }

    private List<Subscription> getSubscriptions() {
        Subscription[] subscriptionsResponse = subscriptionsApiWebClient.get()
                .uri(uriBuilder -> uriBuilder
                        .path("/subscriptions")
                        .build())
                .retrieve()
                .bodyToMono(Subscription[].class)
                .block();
        return subscriptionsResponse != null
                ? Arrays.stream(subscriptionsResponse).collect(Collectors.toList()) : Collections.emptyList();
    }

    private Ticker getCryptoChanges(CryptocurrencySymbol symbol) {
        Optional<Ticker> ticker = binanceWebClient.get()
                .uri(uriBuilder -> uriBuilder
                        .path("/v3/ticker/24hr")
                        .queryParam("symbol", symbol.getBaseAsset() + symbol.getQuoteAsset())
                        .build())
                .retrieve()
                .bodyToMono(Ticker.class)
                .blockOptional();
        return ticker.orElse(null);
    }

}
