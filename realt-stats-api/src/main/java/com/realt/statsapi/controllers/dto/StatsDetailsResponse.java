package com.realt.statsapi.controllers.dto;

import com.realt.statsapi.data.DailyDetails;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

@AllArgsConstructor
public class StatsDetailsResponse {
    @Getter
    @Setter
    private DailyDetails[] items;
}
