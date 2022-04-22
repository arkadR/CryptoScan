package com.cryptoscan.messaging.service;

import com.cryptoscan.messaging.model.news.News;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.mail.javamail.MimeMessageHelper;
import org.springframework.mail.javamail.MimeMessagePreparator;
import org.springframework.stereotype.Service;
import org.thymeleaf.context.Context;
import org.thymeleaf.spring5.SpringTemplateEngine;

import java.util.List;

@Service
@RequiredArgsConstructor
@Slf4j
public class MailService {

    private final JavaMailSender mailSender;

    private final SpringTemplateEngine templateEngine;

    private String createHtmlBody(String currency, List<News> news) {
        Context context = new Context();
        context.setVariable("currency", currency);
        context.setVariable("news", news);
        return templateEngine.process("mail-template.html", context);
    }

    public void sendNews(String email, String currency, List<News> news) {
        String htmlBody = createHtmlBody(currency, news);
        MimeMessagePreparator preparator = mimeMessage -> {
            MimeMessageHelper mimeMessageHelper = new MimeMessageHelper(mimeMessage, false);
            mimeMessageHelper.setTo(email);
            mimeMessageHelper.setSubject("The latest news on " + currency);
            mimeMessageHelper.setText(htmlBody, true);
            mailSender.send(mimeMessage);
        };
        mailSender.send(preparator);
        log.info("Mail sent to " + email);
    }

}
