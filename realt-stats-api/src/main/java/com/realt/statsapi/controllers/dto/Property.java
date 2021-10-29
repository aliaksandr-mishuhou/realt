package com.realt.statsapi.controllers.dto;

import lombok.Getter;
import lombok.Setter;
import org.springframework.format.annotation.DateTimeFormat;

import java.time.LocalDate;

public class Property {
    @Getter
    @Setter
    private long id;
    @Getter
    @Setter
    private Integer roomTotal;
    @Getter
    @Setter
    private Integer roomSeparate;
//    @Getter
//    @Setter
//    private Boolean shared;
    @Getter
    @Setter
    private Integer year;
    @Getter
    @Setter
    private Integer yearFrom;
    @Getter
    @Setter
    private Integer yearTo;
//    @Getter
//    @Setter
//    private Boolean isNew;
    @Getter
    @Setter
    private Double squareTotal;
    @Getter
    @Setter
    private Double squareLiving;
    @Getter
    @Setter
    private Double squareKitchen;
    @Getter
    @Setter
    private Integer floor;
    @Getter
    @Setter
    private Integer floorTotal;
    @Getter
    @Setter
    private String type;
    @Getter
    @Setter
    private String balcony;
    @Getter
    @Setter
    private String district;
    @Getter
    @Setter
    private String address;
    @Getter
    @Setter
    private Integer priceUsd;
    @Getter
    @Setter
    private Integer priceByn;
    @Getter
    @Setter
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME)
    private LocalDate created;
}
