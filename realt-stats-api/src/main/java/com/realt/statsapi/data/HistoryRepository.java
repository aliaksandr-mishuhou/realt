package com.realt.statsapi.data;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.repository.query.Param;
import org.springframework.transaction.annotation.Transactional;

public interface HistoryRepository extends JpaRepository<HistoryItem, HistoryItemId> {
    @Transactional
    Long deleteByScanIdAndSource(
            @Param("scanId") String scanId,
            @Param("source") int source);
}
