package com.cryptoscan.messaging.model.mail;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

@Getter
@Setter
@ToString
public class EmailMessage {
    private String email;
    private List<CurrencyNews> currencyNews;

    public EmailMessage(String email) {
        this.email = email;
        this.currencyNews = new ArrayList<>();
    }

    public List<String> getCurrencies() {
        return currencyNews.stream().map(CurrencyNews::getCurrency).collect(Collectors.toList());
    }
}
