package com.cryptoscan.messaging.queue;

import com.cryptoscan.messaging.model.mail.CurrencyNews;
import com.cryptoscan.messaging.model.mail.EmailMessage;
import com.cryptoscan.messaging.model.message.CryptoChangesMessage;
import com.cryptoscan.messaging.model.news.News;
import com.cryptoscan.messaging.service.MailService;
import com.cryptoscan.messaging.service.NewsService;
import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.Map;

@Component
@RequiredArgsConstructor
public class QueueConsumer {

    private final NewsService newsService;

    private final MailService mailService;

    @RabbitListener(queues = "cryptoChanges")
    public void receive(CryptoChangesMessage message) {
        EmailMessage emailMessage = new EmailMessage(message.getEmail());
        List<CurrencyNews> currencyNews = emailMessage.getCurrencyNews();
        Map<String, List<News>> news = newsService.getNewsForCurrency(message.getCurrencies());
        news.forEach((key, value) -> currencyNews.add(new CurrencyNews(key, value)));
        mailService.sendNews(emailMessage);
    }

}
