package com.realt.statsapi.data;

import org.springframework.data.jpa.repository.JpaRepository;

import java.time.LocalDate;

public interface StatsCountRepository extends JpaRepository<DailyCountItem, LocalDate> {
}
