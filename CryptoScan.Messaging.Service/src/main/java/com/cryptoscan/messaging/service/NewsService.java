package com.cryptoscan.messaging.service;

import com.cryptoscan.messaging.model.news.News;
import com.cryptoscan.messaging.model.news.NewsResponse;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.reactive.function.client.WebClient;

import java.util.*;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class NewsService {

    @Value("${cryptopanic.token}")
    private String cryptopanicToken;

    private final WebClient webClient;

    public Map<String, List<News>> getNewsForCurrency(Set<String> currencies) {
        Optional<NewsResponse> newsResponse = webClient.get()
                .uri(uriBuilder -> uriBuilder
                        .queryParam("auth_token", cryptopanicToken)
                        .queryParam("currencies", String.join(",", currencies))
                        .queryParam("kind", "news")
                        .build())
                .retrieve()
                .bodyToMono(NewsResponse.class)
                .blockOptional();
        Map<String, List<News>> newsToSend = new HashMap<>();
        List<News> news = newsResponse.map(response -> response.getNews().stream().collect(Collectors.toList()))
                .orElse(Collections.emptyList());
        for (String currency : currencies) {
            List<News> newsForCurrency = news.stream().filter(n -> n.getCurrencies().stream()
                    .anyMatch(c -> currency.equals(c.getCode()))).limit(5).collect(Collectors.toList());
            newsToSend.put(currency, newsForCurrency);
        }
        return newsToSend;
    }

}
