-- Table: public.history

-- DROP TABLE public.history;

CREATE TABLE public.history
(
    id bigint NOT NULL,
    scan_id character varying(50) COLLATE pg_catalog."default" NOT NULL,
    scanned timestamp without time zone,
    room_total smallint,
    room_separate smallint,
    year smallint,
    square_total double precision,
    square_living double precision,
    square_kitchen double precision,
    floor smallint,
    floor_total smallint,
    price_usd integer,
    price_byn integer,
    type character varying(10) COLLATE pg_catalog."default",
    balcony character varying(10) COLLATE pg_catalog."default",
    district character varying(20) COLLATE pg_catalog."default",
    address character varying(100) COLLATE pg_catalog."default",
    created date,
    error character varying COLLATE pg_catalog."default",
    CONSTRAINT history_pkey PRIMARY KEY (id, scan_id)
)

TABLESPACE pg_default;

ALTER TABLE public.history
    OWNER to realt;