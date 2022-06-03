package com.cryptoscan.messaging.model.mail;

import com.cryptoscan.messaging.model.news.News;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import java.util.List;

@Getter
@Setter
@ToString
@NoArgsConstructor
public class CurrencyNews {
    private String currency;
    private List<News> news;

    public CurrencyNews(String currency, List<News> news) {
        this.currency = currency;
        this.news = news;
    }
}
