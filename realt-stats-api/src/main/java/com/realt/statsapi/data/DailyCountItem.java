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
@Subselect("SELECT \n" +
        " scan_id as day, \n" +
        " COUNT(*) as total,\n" +
        " COUNT(*) filter (where room_total = 1) as total1,\n" +
        " COUNT(*) filter (where room_total = 2) as total2,\n" +
        " COUNT(*) filter (where room_total = 3) as total3,\n" +
        " COUNT(*) filter (where room_total = 4) as total4,\n" +
        " COUNT(*) filter (where room_total > 4) as total5plus\n" +
        "FROM history\n" +
        "WHERE room_total IS NOT NULL\n" +
        "GROUP BY scan_id\n" +
        "ORDER BY scan_id")
public class DailyCountItem {
    @Id @Getter @Setter
    private LocalDate day;

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
