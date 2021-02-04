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
@Subselect("SELECT scan_id as day, COUNT(*) as total \n" +
        "FROM history\n" +
        "WHERE room_total IS NOT NULL\n" +
        "GROUP BY scan_id\n" +
        "ORDER BY scan_id")
public class DailyItem {
    @Id
    @Getter
    @Setter
    private LocalDate day;

    @Getter
    @Setter
    private int total;
}
