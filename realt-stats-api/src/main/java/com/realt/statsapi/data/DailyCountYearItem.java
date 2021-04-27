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
@Subselect("SELECT * FROM f_daily_count_year()")
public class DailyCountYearItem /*extends DailyCountItem*/ {
    @Id @Getter @Setter
    private LocalDate day;
    @Id @Getter @Setter
    private String years;

    @Getter @Setter
    private int total;
    @Getter @Setter
    private int total1;
    @Getter @Setter
    private int total2;
    @Getter @Setter
    private int total3;
    @Getter @Setter
    private int total4;
    @Getter @Setter
    private int total5plus;
}
