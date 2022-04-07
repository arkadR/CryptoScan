package com.cryptoscan.messaging.config;

import com.cryptoscan.messaging.queue.QueueConsumer;
import org.springframework.amqp.core.Queue;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class QueueConfig {

    @Bean
    public Queue cryptoChanges() {
        return new Queue("cryptoChanges");
    }

    @Bean
    public QueueConsumer queueConsumer() {
        return new QueueConsumer();
    }

}
