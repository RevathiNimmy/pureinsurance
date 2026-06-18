SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_travel_add'
GO

CREATE PROCEDURE spe_travel_add
    @insurance_file_cnt int,
    @scheme_type varchar(20),
    @trip_option varchar(30),
    @area varchar(20),
    @is_baggage_and_money tinyint,
    @name_of_insured varchar(60),
    @age_of_insured int,
    @is_premium_person tinyint,
    @is_golf_cover tinyint,
    @date_of_departure datetime,
    @travel_period int,
    @travel_period_qualifier varchar(20)
AS
BEGIN
INSERT INTO travel (
    insurance_file_cnt ,
    scheme_type ,
    trip_option ,
    area ,
    is_baggage_and_money ,
    name_of_insured ,
    age_of_insured ,
    is_premium_person ,
    is_golf_cover ,
    date_of_departure ,
    travel_period ,
    travel_period_qualifier )
VALUES (
    @insurance_file_cnt,
    @scheme_type,
    @trip_option,
    @area,
    @is_baggage_and_money,
    @name_of_insured,
    @age_of_insured,
    @is_premium_person,
    @is_golf_cover,
    @date_of_departure,
    @travel_period,
    @travel_period_qualifier)
END

GO

