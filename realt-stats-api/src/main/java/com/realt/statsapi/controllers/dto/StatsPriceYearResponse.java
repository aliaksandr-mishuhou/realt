package com.realt.statsapi.controllers.dto;

import com.realt.statsapi.data.DailyPriceItem;
import com.realt.statsapi.data.DailyPriceYearItem;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

@AllArgsConstructor
public class StatsPriceYearResponse {
    @Getter
    @Setter
    private DailyPriceYearItem[] items;
}
