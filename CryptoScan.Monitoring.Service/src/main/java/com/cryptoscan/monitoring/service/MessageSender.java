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

import java.util.*;
import java.util.function.Function;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
@Slf4j
public class MessageSender {

    private static final long SCHEDULE_INTERVAL = 30000L;

    private final RabbitTemplate rabbitTemplate;

    private final Queue queue;

    @Qualifier("subscriptionsApiWebClient")
    private final WebClient subscriptionsApiWebClient;

    @Qualifier("binanceWebClient")
    private final WebClient binanceWebClient;

    private final MonitoringService monitoringService;

    @Scheduled(fixedRate = SCHEDULE_INTERVAL, initialDelay = 10000L)
    public void monitoringTask() {
        log.info("Running scheduled process...");
        Map<Subscription, Ticker> subscriptionsToTickers = getSubscriptions().stream()
                .collect(Collectors.toMap(Function.identity(), subscription -> getCryptoChanges(subscription.getSymbol())));
        List<CryptoChangesMessage> messages = monitoringService.getMessages(subscriptionsToTickers);
        log.info("...Sending " + messages.size() + " messages");
        messages.forEach(message -> {
            rabbitTemplate.convertAndSend(queue.getName(), message);
            log.info("...Message sent to queue " + queue.getName() + ": " + message);
        });
    }

    private List<Subscription> getSubscriptions() {
        Subscription[] subscriptionsResponse = subscriptionsApiWebClient.get()
                .uri(uriBuilder -> uriBuilder
                        .path("/subscriptions")
                        .build())
                .retrieve()
                .bodyToMono(Subscription[].class)
                .block();
        // log.info("Fetched " + subscriptionsResponse.length() + " subscriptions");
        return subscriptionsResponse != null
                ? Arrays.stream(subscriptionsResponse).collect(Collectors.toList()) 
                : Collections.emptyList();
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
        log.info(ticker.toString());
        return ticker.orElse(null);
    }

}
