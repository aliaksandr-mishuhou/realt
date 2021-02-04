package com.realt.statsapi.data;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.*;

@Entity
@Table(name="search")
public class Search {
    @Id
    @GeneratedValue(strategy= GenerationType.AUTO)
    @Getter
    @Setter
    private int id;

    @Getter
    @Setter
    private String data;

    @Getter
    @Setter
    private int status;
}
