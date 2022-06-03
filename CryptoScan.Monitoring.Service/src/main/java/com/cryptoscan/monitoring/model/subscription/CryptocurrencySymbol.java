package com.cryptoscan.monitoring.model.subscription;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class CryptocurrencySymbol {
    private String Symbol;
    private String BaseAsset;
    private String QuoteAsset;
}
