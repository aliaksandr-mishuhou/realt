package com.realt.statsapi.data;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.repository.query.Param;

import java.time.LocalDate;
import java.util.List;

public interface StatsDetailsRepository extends JpaRepository<DailyDetails, LocalDate> {

    List<DailyDetails> findByDay(@Param("day") String day);
}
