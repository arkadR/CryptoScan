package com.cryptoscan.messaging.queue;

import org.springframework.amqp.rabbit.annotation.RabbitHandler;
import org.springframework.amqp.rabbit.annotation.RabbitListener;

@RabbitListener(queues = "cryptoChanges")
public class QueueConsumer {

    @RabbitHandler
    public void receive(byte[] message) {
        System.out.println(new String(message));
    }

}
