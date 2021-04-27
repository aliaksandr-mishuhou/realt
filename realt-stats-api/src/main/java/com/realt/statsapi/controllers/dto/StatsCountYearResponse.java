package com.realt.statsapi.controllers.dto;

import com.realt.statsapi.data.DailyCountItem;
import com.realt.statsapi.data.DailyCountYearItem;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

@AllArgsConstructor
public class StatsCountYearResponse {
    @Getter
    @Setter
    private DailyCountYearItem[] items;
}
