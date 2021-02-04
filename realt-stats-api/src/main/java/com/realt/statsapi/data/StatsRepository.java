package com.realt.statsapi.data;

import org.springframework.data.jpa.repository.JpaRepository;

import java.time.LocalDate;

public interface StatsRepository extends JpaRepository<DailyItem, LocalDate> {
}
