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
public class DailyYearId implements Serializable {
    @Getter
    @Setter
    private LocalDate day;

    @Getter
    @Setter
    private String years;

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        DailyYearId that = (DailyYearId) o;
        return day.equals(that.day) && years.equals(that.years);
    }

    @Override
    public int hashCode() {
        return Objects.hash(day, years);
    }
}
