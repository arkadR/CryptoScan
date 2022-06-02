package com.cryptoscan.messaging.service;

import com.cryptoscan.messaging.model.mail.CurrencyNews;
import com.cryptoscan.messaging.model.mail.EmailMessage;
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

    private String createHtmlBody(List<CurrencyNews> currencyNews) {
        Context context = new Context();
        context.setVariable("currencyNews", currencyNews);
        return templateEngine.process("mail-template.html", context);
    }

    public void sendNews(EmailMessage emailMessage) {
        String htmlBody = createHtmlBody(emailMessage.getCurrencyNews());
        MimeMessagePreparator preparator = mimeMessage -> {
            MimeMessageHelper mimeMessageHelper = new MimeMessageHelper(mimeMessage, false);
            mimeMessageHelper.setTo(emailMessage.getEmail());
            mimeMessageHelper.setSubject("The latest news on " + String.join(", ", emailMessage.getCurrencies()));
            mimeMessageHelper.setText(htmlBody, true);
            mailSender.send(mimeMessage);
        };
        mailSender.send(preparator);
        log.info("Mail sent to " + emailMessage.getEmail());
    }

}
