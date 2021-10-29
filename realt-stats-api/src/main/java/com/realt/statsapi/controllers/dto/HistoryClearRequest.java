package com.realt.statsapi.controllers.dto;

import lombok.Getter;
import lombok.Setter;

public class HistoryClearRequest {
    @Getter
    @Setter
    private String scanId;

    @Getter
    @Setter
    private int source;

    @Override
    public String toString() {
        return "{" +
                "scanId='" + scanId + '\'' +
                ", source=" + source +
                '}';
    }
}
