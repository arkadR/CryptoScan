package com.cryptoscan.messaging.service;

import com.cryptoscan.messaging.model.news.News;
import com.cryptoscan.messaging.model.news.NewsResponse;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.reactive.function.client.WebClient;

import java.util.Collections;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class NewsService {

    @Value("${cryptopanic.token}")
    private String cryptopanicToken;

    private final WebClient webClient;

    public List<News> getNewsForCurrency(String currency) {
        Optional<NewsResponse> newsResponse = webClient.get()
                .uri(uriBuilder -> uriBuilder
                        .queryParam("auth_token", cryptopanicToken)
                        .queryParam("currencies", currency)
                        .queryParam("kind", "news")
                        .build())
                .retrieve()
                .bodyToMono(NewsResponse.class)
                .blockOptional();
        return newsResponse.map(response -> response.getNews().stream().limit(5).collect(Collectors.toList()))
                .orElse(Collections.emptyList());
    }

}
