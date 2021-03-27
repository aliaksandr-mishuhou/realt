package com.realt.statsapi.controllers.dto;

import com.realt.statsapi.data.DailyCountItem;
import com.realt.statsapi.data.DailyPriceItem;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

@AllArgsConstructor
public class StatsPriceResponse {
    @Getter
    @Setter
    private DailyPriceItem[] items;
}
