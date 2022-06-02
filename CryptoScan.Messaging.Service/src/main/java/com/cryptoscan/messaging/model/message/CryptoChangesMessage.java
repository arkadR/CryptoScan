package com.cryptoscan.messaging.model.message;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import java.util.Set;

@Getter
@Setter
@ToString
@NoArgsConstructor
public class CryptoChangesMessage {
    private String email;
    private Set<String> currencies;
}
