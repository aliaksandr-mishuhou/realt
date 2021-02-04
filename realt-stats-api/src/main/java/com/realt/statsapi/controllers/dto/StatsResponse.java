package com.realt.statsapi.controllers.dto;

import com.realt.statsapi.data.DailyItem;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

@AllArgsConstructor
public class StatsResponse {
    @Getter
    @Setter
    private DailyItem[] items;
}
