package com.cryptoscan.messaging.queue;

import com.cryptoscan.messaging.model.news.News;
import com.cryptoscan.messaging.service.NewsService;
import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.annotation.RabbitHandler;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

import java.util.List;

@Component
@RabbitListener(queues = "cryptoChanges")
@RequiredArgsConstructor
public class QueueConsumer {

    private final NewsService newsService;

    @RabbitHandler
    public void receive(byte[] message) {
        String msg = new String(message);
        List<News> news = newsService.getNewsForCurrency(msg);
        news.stream().map(News::getTitle).forEach(System.out::println);
    }

}
