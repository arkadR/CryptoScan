package com.cryptoscan.messaging.model.news;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import java.util.List;

@Getter
@Setter
@ToString
public class NewsResponse {
    @JsonProperty("results")
    private List<News> news;
}
