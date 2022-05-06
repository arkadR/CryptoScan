package com.cryptoscan.monitoring.model.message;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class CryptoChangesMessage {
    private String email;
    private String currency;
}
