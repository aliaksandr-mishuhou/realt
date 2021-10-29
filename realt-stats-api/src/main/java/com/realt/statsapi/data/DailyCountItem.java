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
@Subselect("SELECT * FROM f_daily_count()")
public class DailyCountItem {
    @Id @Getter @Setter
    private LocalDate day;

    @Getter @Setter
    private Integer total;
    @Getter @Setter
    private Integer total1;
    @Getter @Setter
    private Integer total2;
    @Getter @Setter
    private Integer total3;
    @Getter @Setter
    private Integer total4;
    @Getter @Setter
    private Integer total5plus;
}
