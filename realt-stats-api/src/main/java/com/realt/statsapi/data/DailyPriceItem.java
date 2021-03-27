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
        "   scan_id as day, \n" +
        "   ROUND(percentile_disc(0.5) within group (order by price_usd/square_total)) as price,\n" +
        "   ROUND(percentile_disc(0.5) within group (order by price_usd/square_total) \n" +
        "     filter (where room_total = 1)) as price1,\n" +
        "   ROUND(percentile_disc(0.5) within group (order by price_usd/square_total) \n" +
        "     filter (where room_total = 2)) as price2,\n" +
        "   ROUND(percentile_disc(0.5) within group (order by price_usd/square_total) \n" +
        "     filter (where room_total = 3)) as price3,\n" +
        "   ROUND(percentile_disc(0.5) within group (order by price_usd/square_total) \n" +
        "     filter (where room_total = 4)) as price4,\n" +
        "   ROUND(percentile_disc(0.5) within group (order by price_usd/square_total) \n" +
        "     filter (where room_total > 4)) as price5plus\n" +
        "FROM history\n" +
        "WHERE room_total IS NOT NULL \n" +
        "   AND price_usd IS NOT NULL AND price_usd > 0\n" +
        "   AND square_total IS NOT NULL AND square_total > 0\n" +
        "GROUP BY scan_id\n" +
        "ORDER BY scan_id")
public class DailyPriceItem {
    @Id @Getter @Setter
    private LocalDate day;

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
