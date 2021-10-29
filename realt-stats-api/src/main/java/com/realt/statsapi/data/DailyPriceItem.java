package com.realt.statsapi.data;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.Immutable;
import org.hibernate.annotations.Subselect;

import javax.persistence.Entity;
import javax.persistence.Id;
import java.time.LocalDate;

@AllArgsConstructor
@NoArgsConstructor
@Entity
@Immutable
@Subselect("SELECT * FROM f_daily_price()")
public class DailyPriceItem {
    @Id @Getter @Setter
    private LocalDate day;

    @Getter @Setter
    private Integer price;
    @Getter @Setter
    private Integer price1;
    @Getter @Setter
    private Integer price2;
    @Getter @Setter
    private Integer price3;
    @Getter @Setter
    private Integer price4;
    @Getter @Setter
    private Integer price5plus;
}
