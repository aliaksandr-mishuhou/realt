package com.realt.statsapi.data;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.Immutable;
import org.hibernate.annotations.Subselect;

import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.IdClass;
import java.time.LocalDate;

@AllArgsConstructor
@NoArgsConstructor
@Entity
@Immutable
@IdClass(DailyYearId.class)
@Subselect("SELECT * FROM f_daily_price_year()")
public class DailyPriceYearItem /*extends DailyPriceItem*/ {
    @Id @Getter @Setter
    private LocalDate day;
    @Id @Getter @Setter
    private String years;

    @Getter @Setter
    private int price;
    @Getter @Setter
    private int price1;
    @Getter @Setter
    private int price2;
    @Getter @Setter
    private int price3;
    @Getter @Setter
    private int price4;
    @Getter @Setter
    private int price5plus;
}
