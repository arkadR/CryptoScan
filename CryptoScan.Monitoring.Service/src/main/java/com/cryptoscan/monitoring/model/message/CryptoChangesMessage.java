package com.cryptoscan.monitoring.model.message;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import java.util.Set;

@Getter
@Setter
@ToString
public class CryptoChangesMessage {
    private String email;
    private Set<String> currencies;

    public static CryptoChangesMessage fromEmailAndSymbols(String email, Set<String> symbols) {
        CryptoChangesMessage cryptoChangesMessage = new CryptoChangesMessage();
        cryptoChangesMessage.email = email;
        cryptoChangesMessage.currencies = symbols;
        return cryptoChangesMessage;
    }
}
