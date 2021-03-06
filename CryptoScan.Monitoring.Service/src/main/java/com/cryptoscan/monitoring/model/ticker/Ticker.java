package com.cryptoscan.monitoring.model.ticker;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class Ticker {
    private String symbol;
    private double priceChange;
    private double priceChangePercent;
}
