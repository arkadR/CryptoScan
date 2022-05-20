package com.cryptoscan.monitoring.model.message;

import com.cryptoscan.monitoring.model.subscription.Subscription;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class CryptoChangesMessage {
    private String email;
    private String currency;

    public static CryptoChangesMessage fromSubscription(Subscription subscription) {
        CryptoChangesMessage cryptoChangesMessage = new CryptoChangesMessage();
        cryptoChangesMessage.email = subscription.getEmail();
        cryptoChangesMessage.currency = subscription.getSymbol().getBaseAsset();
        return cryptoChangesMessage;
    }
}
