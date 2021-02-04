package com.realt.statsapi.data;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.Immutable;
import org.hibernate.annotations.Subselect;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.IdClass;
import java.time.LocalDate;

@NoArgsConstructor
@Entity
@Immutable
@IdClass(DailyDetailsId.class)
@Subselect("SELECT\n" +
        "\tscan_id as day,\n" +
        "\troom_total as rooms,\n" +
        "\tCOUNT(*) as total,\n" +
        "\tROUND(CAST(AVG(price_usd) as numeric)) as price_avg,\n" +
        "\tpercentile_disc(0.5) within group (order by price_usd) as price_mean,\n" +
        "\tROUND(CAST(AVG(square_total) as numeric), 1) as square_avg,\n" +
        "\tROUND(CAST(percentile_disc(0.5) within group (order by square_total) as numeric), 1) as square_mean\n" +
        "FROM history\n" +
        "WHERE room_total IS NOT NULL\n" +
        "GROUP BY scan_id, room_total\n" +
        "ORDER BY scan_id, room_total")
public class DailyDetails {
    @Id
    @Getter
    @Setter
    private String day;

    @Id
    @Getter
    @Setter
    private int rooms;

    @Getter
    @Setter
    private int total;

    @Getter
    @Setter
    @Column(name = "price_mean")
    private float priceMean;

    @Getter
    @Setter
    @Column(name = "square_mean")
    private float squareMean;

    @Override
    public String toString() {
        return "DailyDetails{" +
                "day=" + day +
                ", total=" + total +
                ", rooms=" + rooms +
                ", priceMean=" + priceMean +
                ", squareMean=" + squareMean +
                '}';
    }
}
