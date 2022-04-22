package com.cryptoscan.messaging.model.news;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import java.util.List;

@Getter
@Setter
@ToString
public class News {
    private String kind;
    private String title;
    private String url;
    @JsonProperty("published_at")
    private String publishedAt;
    private List<NewsCurrency> currencies;
}
