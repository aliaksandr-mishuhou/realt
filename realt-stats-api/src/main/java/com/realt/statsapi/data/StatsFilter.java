package com.realt.statsapi.data;

import lombok.Getter;
import lombok.Setter;
import org.springframework.format.annotation.DateTimeFormat;

import java.time.LocalDate;

public class StatsFilter {

    private final short DefaultSource = 1;

    private final int DefaultDaysCount = 60;

    @Getter
    @Setter
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
    private LocalDate startDate;

    @Getter
    @Setter
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
    private LocalDate endDate;

    @Getter
    @Setter
    private short source;

    public StatsFilter(){
        LocalDate now = LocalDate.now();
        startDate = now.minusDays(DefaultDaysCount);
        endDate = now;
        source = DefaultSource;
    }

    @Override
    public String toString() {
        return "StatsFilter{" +
                "startDate=" + startDate +
                ", endDate=" + endDate +
                ", source=" + source +
                '}';
    }
}
