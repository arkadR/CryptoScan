package com.cryptoscan.monitoring.service;

import com.cryptoscan.monitoring.model.message.CryptoChangesMessage;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.amqp.core.Queue;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
@Slf4j
public class MessageSender {

    private static final int SCHEDULE_INTERVAL = 86400000;

    private final RabbitTemplate rabbitTemplate;

    private final Queue queue;

    @Scheduled(fixedRate = SCHEDULE_INTERVAL)
    public void monitoringTask() {
        CryptoChangesMessage message = new CryptoChangesMessage();
        message.setCurrency("BTC");
        message.setEmail("test@test.test");
        rabbitTemplate.convertAndSend(queue.getName(), message);
        log.info("Message sent to queue " + queue.getName() + ": " + message);
    }

}
