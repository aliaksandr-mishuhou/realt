package com.realt.statsapi.data;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.time.LocalDate;
import java.util.List;

public interface StatsPriceRepository extends JpaRepository<DailyPriceItem, LocalDate> {
    @Query(value = "SELECT * FROM f_daily_price(:source, :start_date, :end_date);", nativeQuery = true)
    List<DailyPriceItem> find(
            @Param("source") short source,
            @Param("start_date") LocalDate startDate,
            @Param("end_date") LocalDate endDate
    );

    @Query(value = "SELECT * FROM f_daily_price_year(:source, :start_date, :end_date);", nativeQuery = true)
    List<DailyPriceYearItem> findWithYears(
            @Param("source") short source,
            @Param("start_date") LocalDate startDate,
            @Param("end_date") LocalDate endDate
    );
}
