package com.realt.statsapi.data;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.io.Serializable;
import java.time.LocalDate;
import java.util.Objects;

@AllArgsConstructor
@NoArgsConstructor
public class DailyDetailsId implements Serializable {
    @Getter
    @Setter
    private String day;

    @Getter
    @Setter
    private int rooms;

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        DailyDetailsId that = (DailyDetailsId) o;
        return rooms == that.rooms && Objects.equals(day, that.day);
    }

    @Override
    public int hashCode() {
        return Objects.hash(day, rooms);
    }
}
