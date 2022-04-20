package com.cryptoscan.messaging.config;

import org.springframework.amqp.core.Queue;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class QueueConfig {

    @Bean
    public Queue cryptoChangesQueue() {
        return new Queue("cryptoChanges");
    }

}
