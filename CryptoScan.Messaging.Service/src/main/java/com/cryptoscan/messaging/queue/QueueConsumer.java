package com.cryptoscan.messaging.queue;

import com.cryptoscan.messaging.model.message.CryptoChangesMessage;
import com.cryptoscan.messaging.model.news.News;
import com.cryptoscan.messaging.service.NewsService;
import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

import java.util.List;

@Component
@RequiredArgsConstructor
public class QueueConsumer {

    private final NewsService newsService;

    @RabbitListener(queues = "cryptoChanges")
    public void receive(CryptoChangesMessage message) {
        List<News> news = newsService.getNewsForCurrency(message.getCurrency());
        news.stream().map(News::getTitle).forEach(System.out::println);
    }

}
