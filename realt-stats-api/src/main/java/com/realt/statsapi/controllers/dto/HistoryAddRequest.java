package com.realt.statsapi.controllers.dto;

import lombok.Getter;
import lombok.Setter;
import org.springframework.format.annotation.DateTimeFormat;

import java.time.LocalDate;
import java.util.Arrays;

public class HistoryAddRequest {
    @Getter
    @Setter
    private String scanId;

    @Getter
    @Setter
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME)
    private LocalDate scanned;

    @Getter
    @Setter
    private int source;

    @Getter
    @Setter
    private Property[] items;

    @Override
    public String toString() {
        return "{" +
                "scanId='" + scanId + '\'' +
                ", source=" + source +
                ", items=" + Arrays.toString(items) +
                '}';
    }
}
