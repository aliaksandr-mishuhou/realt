package com.realt.statsapi.data;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import javax.persistence.Column;
import java.io.Serializable;
import java.util.Objects;

@AllArgsConstructor
@NoArgsConstructor
public class HistoryItemId implements Serializable {
    @Getter
    @Setter
    private long id;

    @Getter
    @Setter
    @Column(name="scan_id")
    private String scanId;

    @Getter
    @Setter
    private int source;

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        HistoryItemId that = (HistoryItemId) o;
        return id == that.id && source == that.source && scanId.equals(that.scanId);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id, scanId, source);
    }
}
