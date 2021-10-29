package com.realt.statsapi.data;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.Immutable;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.IdClass;
import java.time.LocalDate;

@NoArgsConstructor
// TODO: move to config / stored procedure
@Entity(name = "history2")
@Immutable
@IdClass(HistoryItemId.class)
public class HistoryItem {
    @Getter
    @Setter
    @Id
    private long id;

    @Getter
    @Setter
    @Id
    @Column(name="scan_id")
    private String scanId;

    @Getter
    @Setter
    @Id
    private int source;

    @Getter @Setter private Integer roomTotal;
    @Getter @Setter private Integer roomSeparate;
    @Getter @Setter private Integer year;
    @Getter @Setter private Integer yearFrom;
    @Getter @Setter private Integer yearTo;
    @Getter @Setter private Double squareTotal;
    @Getter @Setter private Double squareLiving;
    @Getter @Setter private Double squareKitchen;
    @Getter @Setter private Integer floor;
    @Getter @Setter private Integer floorTotal;
    @Getter @Setter private String type;
    @Getter @Setter private String balcony;
    @Getter @Setter private String district;
    @Getter @Setter private String address;
    @Getter @Setter private Integer priceUsd;
    @Getter @Setter private Integer priceByn;
    @Getter @Setter private LocalDate created;
    @Getter @Setter private LocalDate scanned;

    @Override
    public String toString() {
        return "HistoryItem{" +
                "id=" + id +
                ", scanId='" + scanId + '\'' +
                ", source=" + source +
                ", roomTotal=" + roomTotal +
                ", roomSeparate=" + roomSeparate +
                ", year=" + year +
                ", yearFrom=" + yearFrom +
                ", yearTo=" + yearTo +
                ", squareTotal=" + squareTotal +
                ", squareLiving=" + squareLiving +
                ", squareKitchen=" + squareKitchen +
                ", floor=" + floor +
                ", floorTotal=" + floorTotal +
                ", type='" + type + '\'' +
                ", balcony='" + balcony + '\'' +
                ", district='" + district + '\'' +
                ", address='" + address + '\'' +
                ", priceUsd=" + priceUsd +
                ", priceByn=" + priceByn +
                ", created=" + created +
                ", scanned=" + scanned +
                '}';
    }
}
