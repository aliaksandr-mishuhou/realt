package com.realt.statsapi.controllers.dto;

import com.realt.statsapi.data.DailyCountItem;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

@AllArgsConstructor
public class StatsCountResponse {
    @Getter
    @Setter
    private DailyCountItem[] items;
}
