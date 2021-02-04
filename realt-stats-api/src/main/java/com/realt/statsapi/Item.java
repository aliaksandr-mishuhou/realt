package com.realt.statsapi;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;
import org.springframework.data.annotation.Id;

import java.time.LocalDate;

@AllArgsConstructor
public class Item {
    @Getter
    @Setter
    private LocalDate day;

    @Getter
    @Setter
    private int total;
}
