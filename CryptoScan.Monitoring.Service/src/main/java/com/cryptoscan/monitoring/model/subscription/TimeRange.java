package com.cryptoscan.monitoring.model.subscription;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import java.time.LocalDateTime;

@Getter
@Setter
@ToString
public class TimeRange {
    private LocalDateTime startDate;
    private LocalDateTime endDate;
}
